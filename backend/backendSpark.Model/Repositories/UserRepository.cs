using backendSpark.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace backendSpark.Model.Repositories
{

    // Purpose: This class represents the repository for users. It contains methods to interact with the user table in the database.
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) {}

        // GetUser: Retrieves a user by username and password from the database. It creates a new NpgsqlConnection, executes a SQL command to select the user, and returns a User object if found.
        public virtual User? GetUser(string username, string password)
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

        // CreateUser: Inserts a new user into the database. It creates a new NpgsqlConnection, executes a SQL command to insert the user, and returns true if successful.
        public virtual void CreateUser(User user)
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

// Notes to the code:
    // All functions are virtual so that they can be overridden in the test project.