namespace VLDocesWeb.Models;

public class CartItem
{
    public Product Produto { set; get; }
    public float PrecoVendido { set; get; }
    public int Quantidade { set; get; }
    public int PedidoId { set; get; }
}