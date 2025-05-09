using System.Text.Json.Serialization;

namespace backendSpark.Model.Entities
{
    public class Login
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}