using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VLDocesWeb.Repositories
{
    public class OrderDatabaseRepository : DBConnection, IOrderRepository
    {
        public OrderDatabaseRepository(string? strConn) : base(strConn) { }

        public int Criar(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId)
        {
            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = transaction;
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_RegistraPedido";

                cmd.Parameters.AddWithValue("@total", summary.TotalGeral);
                cmd.Parameters.AddWithValue("@id_cliente", customerId);
                cmd.Parameters.AddWithValue("@id_endereco", enderecoId);
                cmd.Parameters.AddWithValue("@observacoes", (object)payment.Observacoes ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@frete", summary.Frete);
                
                cmd.Parameters.AddWithValue("@forma_pagamento", payment.FormaPagamento);

                // Parâmetros de Encomenda
                if (payment.DataEntregaAgendada.HasValue)
                {
                    cmd.Parameters.AddWithValue("@data_entrega_agendada", payment.DataEntregaAgendada.Value);
                    cmd.Parameters.AddWithValue("@opcao_pagamento_encomenda", payment.OpcaoPagamentoEncomenda);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@data_entrega_agendada", DBNull.Value);
                    cmd.Parameters.AddWithValue("@opcao_pagamento_encomenda", 0);
                }

                object result = cmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    int novoPedidoId = Convert.ToInt32(result); 

                    cmd.CommandType = CommandType.Text;
                    
                    foreach (var item in cart)
                    {
                        cmd.Parameters.Clear();
                        
                        cmd.CommandText = @"
                            INSERT INTO Itens_Pedidos (id_pedido, id_produto, preco_vend, quantidade)
                            VALUES (@id_pedido, @id_produto, @preco_vend, @quantidade)";
                        
                        cmd.Parameters.AddWithValue("@id_pedido", novoPedidoId);
                        cmd.Parameters.AddWithValue("@id_produto", item.Produto.Id_Produto);
                        cmd.Parameters.AddWithValue("@preco_vend", item.PrecoVendido);
                        cmd.Parameters.AddWithValue("@quantidade", item.Quantidade);
                        
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return novoPedidoId;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return 0;
            }
        }

        public List<Order> Listar()
        {
            var lista = new List<Order>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Pedidos ORDER BY id_pedido DESC";

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id_colaborador;
                    if(reader.IsDBNull(reader.GetOrdinal("id_colaborador"))) {
                        id_colaborador = 0;
                    } else {
                        id_colaborador = (int)reader["id_colaborador"];
                    }
                    
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = id_colaborador,
                        }
                    );
                }
            }
            return lista;
        }

        public List<Order> ListarPorStatus(int status)
        {
            var lista = new List<Order>();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT * FROM Pedidos WHERE status = @status ORDER BY id_pedido DESC";
            cmd.Parameters.AddWithValue("@status", status);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id_colaborador;
                    if (reader.IsDBNull(reader.GetOrdinal("id_colaborador")))
                    {
                        id_colaborador = 0;
                    }
                    else
                    {
                        id_colaborador = (int)reader["id_colaborador"];
                    }
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = id_colaborador,
                        }
                    );
                }
            }
            
            return lista;
        }
        
        public List<OrderDetailsModel> GetOrderDetails(int id)
        {
            var detailsList = new List<OrderDetailsModel>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM v_DetalhesPedidos WHERE Pedido = @id";
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var item = new OrderDetailsModel
                {
                    Pedido = (int)reader["Pedido"],
                    Total = (decimal)reader["Total"],
                    Data = (DateTime)reader["Data"],
                    StatusPedido = (string)reader["StatusPedido"],
                    IdCliente = (int)reader["IdCliente"],
                    Cliente = (string)reader["Cliente"],
                    Produto = (string)reader["Produto"],
                    Descricao = (string)reader["Descricao"],
                    PrecoVendido = (decimal)reader["PrecoVendido"],
                    Quantidade = (int)reader["Quantidade"],
                    PrecoItem = (decimal)reader["PrecoItem"],
                    FormaPagamento = (string)reader["FormaPagamento"],
                    StatusPagamento = (string)reader["StatusPagamento"],
                    PrazoEntrega = (DateTime)reader["PrazoEntrega"],
                    StatusEntrega = (string)reader["StatusEntrega"],
                    Endereco = (string)reader["Endereco"],

                    Observacoes = reader["Observacoes"] == DBNull.Value ? "" : (string)reader["Observacoes"],
                    Colaborador = reader["Colaborador"] == DBNull.Value ? "Nenhum" : (string)reader["Colaborador"],
                    DataEntrega = reader["DataEntrega"] == DBNull.Value ? null : (DateTime?)reader["DataEntrega"]
                };
                detailsList.Add(item);
            }
            return detailsList;
        }

        public void UpdateOrderStatus(int orderId, int newStatus)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE Pedidos SET status = @status WHERE id_pedido = @orderId";
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }

        public void UpdatePaymentStatus(int orderId, int newStatus)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE Pagamentos SET status = @status WHERE id_pedido = @orderId";
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }

        public void AssignCollaborator(int orderId, int collaboratorId)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE Pedidos SET id_colaborador = @colabId WHERE id_pedido = @orderId";
            cmd.Parameters.AddWithValue("@colabId", collaboratorId);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }

        public void UpdateDeliveryStatus(int orderId, int newStatus)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            string sqlQuery = @"UPDATE Entregas SET status = @status WHERE id_pedido = @orderId";
            if (newStatus == 1)
            {
                sqlQuery = @"
                    UPDATE Entregas 
                    SET 
                        status = @status, 
                        data_entrega = GETDATE() 
                    WHERE 
                        id_pedido = @orderId";
            }

            cmd.CommandText = sqlQuery;
            cmd.Parameters.AddWithValue("@status", newStatus);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.ExecuteNonQuery();
        }

        public List<Order> ListarPorCliente(int idCliente)
        {
            var lista = new List<Order>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;

            cmd.CommandText = "SELECT * FROM Pedidos WHERE id_cliente = @idCliente ORDER BY id_pedido DESC";
            cmd.Parameters.AddWithValue("@idCliente", idCliente);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id_colaborador;
                    if (reader.IsDBNull(reader.GetOrdinal("id_colaborador")))
                    {
                        id_colaborador = 0;
                    }
                    else
                    {
                        id_colaborador = (int)reader["id_colaborador"];
                    }
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = id_colaborador,
                        }
                    );
                }
            }
            return lista;
        }
        public Order GetById(int id)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Pedidos WHERE id_pedido = @id_pedido";
            cmd.Parameters.AddWithValue("@id_pedido", id);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    int id_colaborador;
                    if (reader.IsDBNull(reader.GetOrdinal("id_colaborador")))
                    {
                        id_colaborador = 0;
                    }
                    else
                    {
                        id_colaborador = (int)reader["id_colaborador"];
                    }
                    return new Order
                    {
                        Id = (int)reader["id_pedido"],
                        Total = (decimal)reader["total"],
                        Data = (DateTime)reader["data_hora"],
                        Status = (int)reader["status"],
                        IdCliente = (int)reader["id_cliente"],
                        IdColaborador = id_colaborador,
                    };
                }
            }
            return null;
        }

        public List<OrderItemViewModel> ListarItensPorPedido(int idPedido)
        {
            var lista = new List<OrderItemViewModel>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            
            cmd.CommandText = @"
                SELECT 
                    p.nome_produto, ip.quantidade, ip.preco_vend 
                FROM 
                    Itens_Pedidos ip  
                INNER JOIN 
                    Produtos p ON ip.id_produto = p.id_produto
                WHERE 
                    ip.id_pedido = @id_pedido";
                    
            cmd.Parameters.AddWithValue("@id_pedido", idPedido);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(
                        new OrderItemViewModel
                        {
                            NomeProduto = (string)reader["nome_produto"],
                            Quantidade = (int)reader["quantidade"],
                            PrecoUnitario = (decimal)reader["preco_vend"], 
                        }
                    );
                }
            }
            return lista;
        }

        public void CancelOrder(int orderId)
        {
            //Para garantir que tudo ou nada será feit
            SqlTransaction transaction = conn.BeginTransaction();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.Transaction = transaction;

            try
            {
                //Stts do pedido para cancelado
                cmd.CommandText = "UPDATE Pedidos SET status = 2 WHERE id_pedido = @orderId";
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.ExecuteNonQuery();

                //stts pagamentyo para cancelado
                cmd.Parameters.Clear();
                cmd.CommandText = "UPDATE Pagamentos SET status = 2 WHERE id_pedido = @orderId";
                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.ExecuteNonQuery();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}