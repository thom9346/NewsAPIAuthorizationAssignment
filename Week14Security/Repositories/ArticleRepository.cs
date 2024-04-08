using Microsoft.EntityFrameworkCore;
using Week14Security.Data;
using Week14Security.Models;
namespace Week14Security.Repositories
{
    public class ArticleRepository : IRepository<Article>
    {
        private readonly ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public Article Add(Article entity)
        {
            var newArticle = _db.Articles.Add(entity).Entity;
            _db.SaveChanges();
            return newArticle;
        }

        public void Edit(Article entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public Article Get(int id)
        {
            return _db.Articles.FirstOrDefault(a => a.ArticleId == id);
        }

        public IEnumerable<Article> GetAll()
        {
            return _db.Articles.ToList();
        }

        public void Remove(int id)
        {
            var article = _db.Articles.FirstOrDefault(a => a.ArticleId == id);
            _db.Articles.Remove(article);
            _db.SaveChanges();
        }
    }
}
