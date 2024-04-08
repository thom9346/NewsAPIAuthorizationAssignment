using Week14Security.Data;
using Week14Security.Models;

namespace Week14Security.Repositories
{
    public class UserRepository : IUserRepository
    {


        private readonly ApplicationDbContext _db;
        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public User Add(User entity)
        {
            var newUser = _db.Users.Add(entity).Entity;
            _db.SaveChanges();
            return newUser;
        }

        public void Edit(User entity)
        {
            throw new NotImplementedException();
        }

        public User FindByUsername(string username)
        {
            return _db.Users.FirstOrDefault(x => x.Username == username); 
        }
     

        public User Get(int id)
        {
            return _db.Users.FirstOrDefault(a => a.UserId == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users.ToList();
        }

        public string GetUserRole(int userId)
        {
            return _db.Users.FirstOrDefault(x => x.UserId == userId).Role;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
