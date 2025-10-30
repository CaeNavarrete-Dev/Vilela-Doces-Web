using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace VLDocesWeb.Repositories;

public class ProdCategorieDatabaseRepository : DBConnection, IProdCategorieRepository
{
    public ProdCategorieDatabaseRepository(string? strConn) : base(strConn) { }

    public List<ProdCategorie> ListAll()
    {
        var list = new List<ProdCategorie>();
        
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT Id_Categoria, Nome FROM Categorias"; 

        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                list.Add(new ProdCategorie
                {
                    Id_Categoria = (int)reader["Id_Categoria"],
                    Nome = (string)reader["Nome"]
                });
            }
        }
        return list;
    }
}