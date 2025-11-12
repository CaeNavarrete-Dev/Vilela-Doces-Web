using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IOrderRepository
{
    int CreateOrder(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId);
    List<Order> Listar();
    void Criar(Order pedido);
    void Update(Order order); // Não consegui
    List<Order> ListarPorStatus(int status);
}