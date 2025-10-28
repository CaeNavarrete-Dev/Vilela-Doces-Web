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

    public List<Product> ListAll()
    {
        throw new NotImplementedException();
    }
}