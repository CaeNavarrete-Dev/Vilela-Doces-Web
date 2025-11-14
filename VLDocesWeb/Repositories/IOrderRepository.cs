namespace VLDocesWeb.Models
{
    public interface IOrderRepository
    {
        List<Order> Listar();
        void Criar(Order pedido);
        void Update(Order order); // Não consegui
        List<Order> ListarPorStatus(int status);
        List<Order> ListarPorCliente(int idCliente);
        Order GetById(int id);
        List<OrderItemViewModel> ListarItensPorPedido(int idPedido);
    }
}