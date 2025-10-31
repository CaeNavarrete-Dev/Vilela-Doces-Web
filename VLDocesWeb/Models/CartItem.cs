namespace VLDocesWeb.Models;

public class CartItem
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; }
    public int Quantidade { get; set; }
    public decimal Preco { get; set; }
    public decimal SubTotal => Quantidade * Preco;
}