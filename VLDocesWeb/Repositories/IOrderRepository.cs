using VLDocesWeb.Models;

namespace VLDocesWeb;

public interface IOrderRepository
{
    void Add(Order order);
    List<Order> ListAll();
}