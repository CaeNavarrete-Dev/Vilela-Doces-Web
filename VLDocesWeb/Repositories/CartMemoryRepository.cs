using VLDocesWeb.Models;
namespace VLDocesWeb.Repositories;

public class CartMemoryRepository
{
    public static List<CartItem> Carrinho = new();

    public void Add(Product product)
    {
        //Procura um item pelo id do produto, se não encontrar retorna null
        var item = Carrinho.FirstOrDefault(c => c.ProdutoId == product.Id_Produto);
        // Se o retorno for null adicionamos o item no carrinho
        if (item == null)
        {
            Carrinho.Add(new CartItem
            {
                ProdutoId = product.Id_Produto,
                Nome = product.Nome,
                Quantidade = 1,
                Preco = (decimal)product.Preco
            });
        }
        else
        {
            item.Quantidade++;
        }
    }

    public void Remove(int idProduto)
    {
        var item = Carrinho.FirstOrDefault(c => c.ProdutoId == idProduto);

        if (item != null)
        {
            Carrinho.Remove(item);
        }
    }
    
    public void Clear()
    {
        Carrinho.Clear();
    }
}