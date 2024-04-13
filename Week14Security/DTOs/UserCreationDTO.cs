namespace Week14Security.DTOs
{
    public class UserCreationDTO
    {
        public string Username { get; set; }
        public string Password { get; set; } //plaintext pw
        public string Role { get; set; }
    }
}
