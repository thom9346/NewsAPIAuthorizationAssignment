
using Week14Security.Models;
using Week14Security.Utility;

namespace Week14Security.Data
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(ApplicationDbContext context)
        {
           context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }
            List<User> users = new List<User>
            {
               new User { UserId = 1, Username = "The_Jorunalist", PasswordHash = PasswordHasher.HashPassword("hej"), Role = "Journalist"},
               new User { UserId = 2, Username = "The_Editor", PasswordHash = PasswordHasher.HashPassword("hej"), Role = "Editor"},
               new User { UserId = 3, Username = "The_Subscriber", PasswordHash = PasswordHasher.HashPassword("hej"), Role = "Subscriber"}
            };
            List<Article> articles = new List<Article>
            {
                new Article {ArticleId = 1, AuthorId = 1, Content = "The patient was left truamatized, but the dentist was quite pleased with the results. 'It worked well' he claimed", Title="Dentist KNOCKS OUT theeth with a Hammer", CreatedAt = DateTime.Now},
                new Article {ArticleId = 2, AuthorId = 1, Content = "We don't really know how", Title="Plumber fixed a toilet with a spoon", CreatedAt= DateTime.Now},
                new Article {ArticleId = 3, AuthorId = 1, Content = "some very good content, jepjepjepjep", Title="A title", CreatedAt = DateTime.Now},

            };
            List<Comment> comments = new List<Comment>
            {
                new Comment {ArticleId = 1, CommentId=1, CreatedAt = DateTime.Now, Content = "fedt"}
            };
            context.Users.AddRange(users);
            context.Articles.AddRange(articles);
            context.Comments.AddRange(comments);
            context.SaveChanges();
        }
    }
}
