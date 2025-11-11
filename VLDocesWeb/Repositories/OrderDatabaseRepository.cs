using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace VLDocesWeb.Repositories
{
    public class OrderDatabaseRepository : DBConnection, IOrderRepository
    {
        public OrderDatabaseRepository(string? strConn) : base(strConn) { }

        public int CreateOrder(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId)
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
    }
}