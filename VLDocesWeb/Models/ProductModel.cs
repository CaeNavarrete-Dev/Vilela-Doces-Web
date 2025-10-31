namespace VLDocesWeb.Models;

public class Product
{
    public int Id_Produto { set; get; }
    public string Nome { set; get; }
    public string Descricao { set; get; }
    public float Preco { set; get; }
    public int Id_Categoria{ set; get; }
}