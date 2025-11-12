using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IOrderRepository
{
    int Criar(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId);
    List<Order> Listar();
    // void Update(Order order); // Não consegui
    List<Order> ListarPorStatus(int status);
}