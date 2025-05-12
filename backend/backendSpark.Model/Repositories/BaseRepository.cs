
// Purpose: Contains the BaseRepository class which is the base class for all the repositories in the project. It contains the connection string and methods to interact with the database.

namespace backendSpark.Model.Repositories;
using Npgsql;
using Microsoft.Extensions.Configuration;

public class BaseRepository
{
    protected string ConnectionString { get; }

    // Constructor for production use
    public BaseRepository(IConfiguration configuration)
    {
        ConnectionString = configuration.GetConnectionString("spark_db");
    }

    // Constructor for testing purposes
        public BaseRepository(string connectionString)
    {
        ConnectionString = connectionString;
    }

    protected virtual NpgsqlDataReader GetData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        return cmd.ExecuteReader();
    }
    protected virtual bool InsertData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    protected virtual bool UpdateData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    protected virtual bool DeleteData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
}


