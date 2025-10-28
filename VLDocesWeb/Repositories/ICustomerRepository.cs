using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public interface ICustomerRepository
{
    User Login(LoginViewModel model);

    void Register(Customer model);
}