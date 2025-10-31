using VLDocesWeb.Models;

namespace VLDocesWeb.Repositories;

public class CustomerMemoryRepository : ICustomerRepository
{
    public static List<Customer> _customers = new List<Customer>();
    public Customer Login(LoginViewModel model)
    {
        foreach (Customer c in _customers)
        {
            if(c.Email == model.Email && c.Senha == model.Senha)
            {
                return c;
            }
        }
                
        return null;
    }

    public void Register(Customer customer)
    {
        Console.WriteLine($"Novo cliente: {customer.Nome} | {customer.Email} | {customer.Senha}");
        customer.Id = _customers.Count + 1;
        _customers.Add(customer);
        return;
    }
}