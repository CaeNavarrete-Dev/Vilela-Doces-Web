namespace VLDocesWeb.Models;

public class CartItem
{
    public Product Produto { set; get; }
    public decimal PrecoVendido { set; get; }
    public int Quantidade { set; get; }
    public int PedidoId { set; get; }
}