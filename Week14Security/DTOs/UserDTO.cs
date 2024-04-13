using System.ComponentModel.DataAnnotations;

namespace Week14Security.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
