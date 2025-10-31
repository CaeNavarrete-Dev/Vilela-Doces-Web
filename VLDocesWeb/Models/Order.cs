namespace VLDocesWeb.Models;

public class Order
{
    public int Id { get; set; }
    public int CustumerId { get; set; }
    public List<CartItem> Items { get; set; } = new();//Lista de itesn daquele pedido
    public string Endereco { get; set; }
    public string MetodoPagamento { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public decimal Total => Items.Sum(i => i.SubTotal);//Soma cada item do pedido
}