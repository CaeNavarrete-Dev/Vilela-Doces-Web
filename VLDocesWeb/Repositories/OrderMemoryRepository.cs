using VLDocesWeb.Models;
namespace VLDocesWeb.Repositories;

public class OrderMemoryRepository : IOrderRepository
{
    private static List<Order> _orders = new();

    public void Add(Order order)
    {
        order.Id = _orders.Count + 1;
        _orders.Add(order);
    }
    
    public List<Order> ListAll()
    {
        return _orders;
    }
}