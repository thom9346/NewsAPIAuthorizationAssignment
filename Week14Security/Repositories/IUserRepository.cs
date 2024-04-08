using Week14Security.Models;

namespace Week14Security.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User Get(int id);
        User Add(User entity);
        void Edit(User entity);
        void Remove(int id);
        User FindByUsername(string username);
        string GetUserRole(int userId);
    }
}
