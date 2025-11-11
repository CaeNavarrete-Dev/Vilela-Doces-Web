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

    public List<Product> ListAllOrder()
    {
        throw new NotImplementedException();
    }

    public List<Product> ListAllPackage()
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Product Read(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(Product model)
    {
        throw new NotImplementedException();
    }

    public ProductMemoryRepository()
    {
     if (_products.Count == 0)
        {
            throw new NotImplementedException();
       }
    }
}