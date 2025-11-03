using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace VLDocesWeb.Repositories;

public class ProductDatabaseRepository : DBConnection, IProductRepository
{
    public ProductDatabaseRepository(string? strConn) : base(strConn)
    {

    }

    public void Create(Product product)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "insert into Produtos (nome_produto, preco, descricao, id_categoria) values (@nome_produto, @preco, @descricao, @id_categoria)";
        cmd.Parameters.AddWithValue("@nome_produto", product.Nome);
        cmd.Parameters.AddWithValue("@preco", product.Preco);
        cmd.Parameters.AddWithValue("@descricao", product.Descricao);
        cmd.Parameters.AddWithValue("@id_categoria", product.Id_Categoria);

        cmd.ExecuteNonQuery();
    }

    public List<Product> ListAllOrder()
    {
        List<Product> _products = new List<Product>();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select p.id_produto, p.nome_produto, p.descricao, p.preco, p.id_categoria from Categorias c, Produtos p where c.id_categoria = p.id_categoria and c.id_categoria = 1";

        SqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            Product product = new Product
            {
                Id_Produto = (int)reader["id_produto"],
                Nome = (string)reader["nome_produto"],
                Preco = (float)Convert.ToDouble(reader["preco"]),
                Id_Categoria = (int)reader["id_categoria"],
                Descricao = (string)reader["descricao"]
            };
            _products.Add(product);
        }
        return _products;
    }

    public List<Product> ListAllPackage()
    {
        List<Product> _products = new List<Product>();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select p.id_produto, p.nome_produto, p.descricao, p.preco, p.id_categoria from Categorias c, Produtos p where c.id_categoria = p.id_categoria and c.id_categoria = 2";

        SqlDataReader reader = cmd.ExecuteReader();
        while(reader.Read())
        {
            Product product = new Product
            {
                Id_Produto = (int)reader["id_produto"],
                Nome = (string)reader["nome_produto"],
                Preco = (float)Convert.ToDouble(reader["preco"]),
                Id_Categoria = (int)reader["id_categoria"],
                Descricao = (string)reader["descricao"]
            };
            _products.Add(product);
        }
        return _products;
    }

    public List<Product> ListAll()
    {
        List<Product> _products = new List<Product>();
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select p.id_produto, p.nome_produto, p.descricao, p.preco, p.id_categoria from Categorias c, Produtos p where c.id_categoria = p.id_categoria";

        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            Product product = new Product
            {
                Id_Produto = (int)reader["id_produto"],
                Nome = (string)reader["nome_produto"],
                Preco = (float)Convert.ToDouble(reader["preco"]),
                Id_Categoria = (int)reader["id_categoria"],
                Descricao = (string)reader["descricao"]
            };
            _products.Add(product);
        }
        return _products;
    }
    
    public void Delete(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "delete from Produtos where id_produto = @id";
        cmd.Parameters.AddWithValue("@id", id);

        cmd.ExecuteNonQuery();
    }

    public Product Read(int id)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "select * from Produtos where id_produto = @id";
        cmd.Parameters.AddWithValue("@id", id);

        SqlDataReader reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new Product
            {
                Id_Produto = (int)reader["id_produto"],
                Nome = (string)reader["nome_produto"],
                Preco = (float)Convert.ToDouble(reader["preco"]),
                Id_Categoria = (int)reader["id_categoria"],
                Descricao = (string)reader["descricao"]
            };
        }
        return null;
    }

    public void Update(Product model)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "update Produtos set nome_produto = @nome_produto, preco = @preco, id_categoria = @id_categoria, descricao = @descricao where id_produto = @id_produto";
        cmd.Parameters.AddWithValue("nome_produto", model.Nome);
        cmd.Parameters.AddWithValue("preco", model.Preco);
        cmd.Parameters.AddWithValue("id_categoria", model.Id_Categoria);
        cmd.Parameters.AddWithValue("descricao", model.Descricao);
        cmd.Parameters.AddWithValue("id_produto", model.Id_Produto);

        cmd.ExecuteNonQuery();
    }

}

