namespace backendSpark.Model.Entities
{
    // Purpose: This class represents a user in the system. It contains properties for the user's ID, username, and password.
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty; 
    }
}