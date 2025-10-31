using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface IProductRepository
{
    public void Create(Product product);

    public List<Product> ListAll();
}