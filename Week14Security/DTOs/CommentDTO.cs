using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Week14Security.Models
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
