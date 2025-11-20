using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;

namespace VLDocesWeb.Repositories;

public class CustomerDatabaseRepository : DBConnection, ICustomerRepository
{
    public CustomerDatabaseRepository(string? strConn) : base(strConn)
    {
        
    }
    public User Login(LoginViewModel model)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select p.id_pessoa, p.nome, p.telefone, p.email, p.tipo_pessoa, c.CPF from Pessoas p LEFT JOIN Clientes c on p.id_pessoa = c.id_pessoa where p.email = @email and p.senha = @senha";
        cmd.Parameters.AddWithValue("email", model.Email);
        cmd.Parameters.AddWithValue("senha", model.Senha);

        SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            var tipo_pessoa = (int)reader["tipo_pessoa"];

            if (tipo_pessoa == 1)
            {
                return new Customer
                {
                    Id = (int)reader["id_pessoa"],
                    Email = (string)reader["email"],
                    Nome = (string)reader["nome"],
                    CPF = (string)reader["CPF"],
                    Telefone = (string)reader["telefone"]
                };
            }
            else
            {
                return new Collaborator
                {
                    Id = (int)reader["id_pessoa"],
                    Email = (string)reader["email"],
                    Nome = (string)reader["nome"],
                    Telefone = (string)reader["telefone"]
                };
            }
        }
        return null;
    }

    public void Register(Customer model)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandType = System.Data.CommandType.StoredProcedure;
        cmd.CommandText = "p_RegistraCliente";
        cmd.Parameters.AddWithValue("nome", model.Nome);
        cmd.Parameters.AddWithValue("telefone", model.Telefone);
        cmd.Parameters.AddWithValue("email", model.Email);
        cmd.Parameters.AddWithValue("senha", model.Senha);
        cmd.Parameters.AddWithValue("cpf", model.CPF);

        cmd.ExecuteNonQuery();
    }
}