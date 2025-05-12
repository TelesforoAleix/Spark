using System.Text.Json.Serialization;

namespace backendSpark.Model.Entities
{
    // Purpose: This class represents the login credentials of a user. It contains properties for the username and password.
    public class Login
    {
        // JsonPropertyName attribute is used to specify the name of the property when serialized to JSON.

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}