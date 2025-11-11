using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IProductRepository
{
    public void Create(Product product);

    public List<Product> ListAllOrder();

    public List<Product> ListAllPackage();

    public List<Product> ListAll();

    public void Delete(int id);

    public Product Read(int id);

    public void Update(Product model);
}