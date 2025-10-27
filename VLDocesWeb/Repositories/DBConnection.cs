using Microsoft.Data.SqlClient;

namespace VLDocesWeb.Repositories;

public abstract class DBConnection : IDisposable
{
    protected SqlConnection conn;

    public DBConnection(string connStr)
    {
        this.conn = new SqlConnection(connStr);
        conn.Open();
    }
    public void Dispose()
    {
        conn.Close();
    }
}

