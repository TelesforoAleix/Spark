
// Purpose: Contains the BaseRepository class which is the base class for all the repositories in the project. It contains the connection string and methods to interact with the database.
// Purpose: Base class for all repositories — provides access to the connection string and helper methods.


namespace backendSpark.Model.Repositories;
using Npgsql;
using Microsoft.Extensions.Configuration;

public class BaseRepository
{
    protected string ConnectionString { get; }
    public BaseRepository(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("spark_db");
    }
    protected NpgsqlDataReader GetData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        return cmd.ExecuteReader();
    }
    protected bool InsertData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    protected bool UpdateData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    protected bool DeleteData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
}


