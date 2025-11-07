using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IOrderRepository
{
    int CreateOrder(List<CartItem> cart, PaymentSummaryViewModel summary, PaymentSubmissionViewModel payment, int customerId, int enderecoId);
}