using Npgsql;
using Microsoft.Extensions.Configuration;

namespace backendSpark.Model.Repositories;

// Purpose: Contains the BaseRepository class which is the base class for all the repositories in the project. It contains the connection string and methods to interact with the database.
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

    // GetData: Executes a SQL command and returns a NpgsqlDataReader object. It opens the connection, executes the command, and returns the data reader.
    protected virtual NpgsqlDataReader GetData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        return cmd.ExecuteReader();
    }
    
    // InsertData: Executes a SQL command to insert data into the database. It opens the connection, executes the command, and returns true if successful.
    protected virtual bool InsertData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    
    // UpdateData: Executes a SQL command to update data in the database. It opens the connection, executes the command, and returns true if successful.
    protected virtual bool UpdateData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
    
    // DeleteData: Executes a SQL command to delete data from the database. It opens the connection, executes the command, and returns true if successful.
    protected virtual bool DeleteData(NpgsqlConnection conn, NpgsqlCommand cmd)
    {
        conn.Open();
        cmd.ExecuteNonQuery();
        return true;
    }
}

// Notes to the code:
    // All functions are virtual so that they can be overridden by other classes.
