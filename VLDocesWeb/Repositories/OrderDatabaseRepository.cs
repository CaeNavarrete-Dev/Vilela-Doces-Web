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
                cmd.Parameters.AddWithValue("@forma_pagamento", payment.FormaPagamento);
                cmd.Parameters.AddWithValue("@id_endereco", enderecoId);
                cmd.Parameters.AddWithValue("@observacoes", payment.Observacoes);
                cmd.Parameters.AddWithValue("@frete", summary.Frete);

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
    }
}