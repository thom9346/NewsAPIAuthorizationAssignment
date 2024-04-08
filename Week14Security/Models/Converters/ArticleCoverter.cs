using Week14Security.DTOs;

namespace Week14Security.Models.Converters
{
    public class ArticleCoverter : IConverter<Article, ArticleDTO>
    {
        public Article Convert(ArticleDTO model)
        {
            return new Article
            {
                ArticleId = model.ArticleId,
                Title = model.Title,
                Content = model.Content,
                AuthorId = model.AuthorId,
                CreatedAt = model.CreatedAt
            };
        }

        public ArticleDTO Convert(Article model)
        {
            return new ArticleDTO
            {
                ArticleId = model.ArticleId,
                Title = model.Title,
                Content = model.Content,
                AuthorId = model.AuthorId,
                CreatedAt = model.CreatedAt
            };
        }
    }
}
