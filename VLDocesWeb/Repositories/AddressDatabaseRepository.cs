using VLDocesWeb.Models;
using Microsoft.Data.SqlClient;

namespace VLDocesWeb.Repositories;

public class AddressDatabaseRepository : DBConnection, IAddressRepository {
    public AddressDatabaseRepository(string? strConn) : base(strConn) {
    }

    public void Create(Address model)
    {
        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "INSERT INTO Enderecos (nome, rua, numero, bairro, cep, complemento, cidade, uf, id_cliente) VALUES (@nome, @rua, @numero, @bairro, @cep, @complemento, @cidade, @uf, @clienteId)";
        cmd.Parameters.AddWithValue("nome", model.Nome);
        cmd.Parameters.AddWithValue("rua", model.Rua);
        cmd.Parameters.AddWithValue("numero", model.Numero);
        cmd.Parameters.AddWithValue("bairro", model.Bairro);
        cmd.Parameters.AddWithValue("cep", model.Cep);
        cmd.Parameters.AddWithValue("cidade", model.Cidade);
        cmd.Parameters.AddWithValue("uf", model.Uf);
        cmd.Parameters.AddWithValue("clienteId", model.ClienteId);

        object complementoValue;
        if (string.IsNullOrWhiteSpace(model.Complemento)) {
            complementoValue = DBNull.Value;
        }
        else {
            complementoValue = model.Complemento;
        }
        cmd.Parameters.AddWithValue("@complemento", complementoValue);

        cmd.ExecuteNonQuery();
    }

    public List<Address> ReadAll(int usuarioId)
    {
        List<Address> lista = new List<Address>();

        SqlCommand cmd = new SqlCommand();
        cmd.Connection = conn;
        cmd.CommandText = "SELECT id_endereco, nome, rua, numero, bairro, cep, complemento, cidade, uf FROM Enderecos WHERE id_cliente = @usuarioId";

        cmd.Parameters.AddWithValue("usuarioId", usuarioId);

        SqlDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string complemento;
            if (reader.IsDBNull(reader.GetOrdinal("complemento"))) {
                complemento = null;
            } else {
                complemento = (string)reader["complemento"];
            }

            lista.Add(new Address
            {
                AddressId = (int)reader["id_endereco"],
                Nome = (string)reader["nome"],
                Rua = (string)reader["rua"],
                Numero = (string)reader["numero"],
                Bairro = (string)reader["bairro"],
                Cep = (string)reader["cep"],
                Complemento = complemento,
                Cidade = (string)reader["cidade"],
                Uf = (string)reader["uf"]
            });
        }

        return lista;
    }
}