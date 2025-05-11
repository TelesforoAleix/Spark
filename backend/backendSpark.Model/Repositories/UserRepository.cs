using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace backendSpark.Model.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) {}

        public User? GetUser(string username, string password)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM \"user\" WHERE username = @username AND password = @password";
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
            return new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Username = reader.GetString(reader.GetOrdinal("username")),
                Password = reader.GetString(reader.GetOrdinal("password"))
            };
            }

            return null;
        }

        public void CreateUser(User user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO \"user\" (username, password) VALUES (@username, @password)";
            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);

            cmd.ExecuteNonQuery();
        }
    }
}