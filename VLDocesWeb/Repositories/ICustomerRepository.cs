using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface ICustomerRepository
{
    Customer Login(LoginViewModel model);

    void Register(Customer model);
}