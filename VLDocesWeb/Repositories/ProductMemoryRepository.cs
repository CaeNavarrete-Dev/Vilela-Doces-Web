using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public class ProductMemoryRepository : IProductRepository
{
    private static List<Product> _products = new List<Product>(); //Lista de produtos


    public void Create(Product product)
    {
        product.Id_Produto = _products.Count + 1;
        _products.Add(product);
    }

    public List<Product> ListAll()
    {
        return _products;
    }
    
public ProductMemoryRepository()
    {
     if (_products.Count == 0)
     {
        Create(new Product
        {
            Nome = "Brigadeiro Tradicional",
            Descricao = "Delicioso brigadeiro de chocolate com granulado macio.",
            Preco = 3.50f,
            Id_Categoria = 1 // Assumindo que 1 = Doces Tradicionais
        });

        Create(new Product
        {
            Nome = "Beijinho",
            Descricao = "Docinho de coco ralado com cravo.",
            Preco = 3.00f,
            Id_Categoria = 1
        });
        
        Create(new Product
        {
            Nome = "Beijinho",
            Descricao = "Docinho de coco ralado com cravo.",
            Preco = 3.00f,
            Id_Categoria = 1
        });
     }
    }
}