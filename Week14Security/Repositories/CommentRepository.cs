using Week14Security.Data;
using Week14Security.Models;

namespace Week14Security.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationDbContext _db;
        public CommentRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public Comment Add(Comment entity)
        {
            var newComment = _db.Comments.Add(entity).Entity;
            _db.SaveChanges();
            return newComment;
        }

        public void Edit(Comment entity)
        {
            throw new NotImplementedException();
        }

        public Comment Get(int id)
        {
            return _db.Comments.FirstOrDefault(a => a.ArticleId == id);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _db.Comments.ToList();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
