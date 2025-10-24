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
            Nome = "Pão de Mel Gourmet",
            Descricao = "Delicioso com cobertura de chocolate e recheio cremoso.",
            Preco = 6.0f,
            Id_Categoria = 1 // Assumindo que 1 = Doces Tradicionais
        });

        Create(new Product
        {
            Nome = "Torta de Limão Siciliano",
            Descricao = "Pedaço de torta de limão siciliano cremosa.",
            Preco = 35.00f,
            Id_Categoria = 1
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