using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IOrderRepository
{
    int Criar(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId);
    List<Order> Listar();
    // void Update(Order order); // Não consegui
    List<Order> ListarPorStatus(int status);

    List<OrderDetailsModel> GetOrderDetails(int id);

    void UpdateOrderStatus(int orderId, int newStatus);

    void UpdatePaymentStatus(int orderId, int newStatus);

    void AssignCollaborator(int orderId, int collaboratorId);
    
    void UpdateDeliveryStatus(int orderId, int newStatus);
}