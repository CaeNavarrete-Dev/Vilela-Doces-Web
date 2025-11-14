using Microsoft.Data.SqlClient;
using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories
{
    public class OrderRepository : DBConnection, IOrderRepository 
    {
        public OrderRepository(string? strConn): base(strConn) { }

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
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = (int)reader["id_colaborador"],
                        }
                    );
                }
            }
            return lista;
        }
        public void Criar(Order pedido)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO Pedidos (total, data_hora, status, id_cliente, id_colaborador) VALUES (@total, @data_hora, @status, @id_cliente, @id_colaborador)";
            cmd.Parameters.AddWithValue("total", pedido.Total);
            cmd.Parameters.AddWithValue("@data_hora", pedido.Data);
            cmd.Parameters.AddWithValue("@status", pedido.Status);
            cmd.Parameters.AddWithValue("@id_cliente", pedido.IdCliente);
            cmd.Parameters.AddWithValue("@id_colaborador", pedido.IdColaborador);

            cmd.ExecuteNonQuery();
        }

        public void Update(Order order){} //não consegui


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
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = (int)reader["id_colaborador"],
                        }
                    );
                }
            }
            return lista;
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
                    lista.Add(
                        new Order
                        {
                            Id = (int)reader["id_pedido"],
                            Total = (decimal)reader["total"],
                            Data = (DateTime)reader["data_hora"],
                            Status = (int)reader["status"],
                            IdCliente = (int)reader["id_cliente"],
                            IdColaborador = (int)reader["id_colaborador"],
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
                    return new Order
                    {
                        Id = (int)reader["id_pedido"],
                        Total = (decimal)reader["total"],
                        Data = (DateTime)reader["data_hora"],
                        Status = (int)reader["status"],
                        IdCliente = (int)reader["id_cliente"],
                        IdColaborador = (int)reader["id_colaborador"],
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
                            NomeProduto = (string)reader["nome"],
                            Quantidade = (int)reader["quantidade"],
                            PrecoUnitario = (decimal)reader["preco_vend"], 
                        }
                    );
                }
            }
            return lista;
        }
    }
}
