namespace Week14Security.Models.Converters
{
    public class Commentconverter : IConverter<Comment, CommentDTO>
    {
        public Comment Convert(CommentDTO model)
        {
            return new Comment
            {
                CommentId = model.CommentId,
                Content = model.Content,
                ArticleId = model.ArticleId,
                UserId = model.UserId,
                CreatedAt = model.CreatedAt,
            };
        }

        public CommentDTO Convert(Comment model)
        {
            return new CommentDTO
            {
                CommentId = model.CommentId,
                Content = model.Content,
                ArticleId = model.ArticleId,
                UserId = model.UserId,
                CreatedAt = model.CreatedAt,
            };
        }
    }
}
