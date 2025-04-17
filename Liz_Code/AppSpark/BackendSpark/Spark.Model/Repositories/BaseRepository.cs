using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Spark.Model.Repositories;


public class BaseRepository
{
    protected string ConnectionString {get;}
public BaseRepository(IConfiguration configuration) {
       ConnectionString = configuration.GetConnectionString("SparkDb");
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
