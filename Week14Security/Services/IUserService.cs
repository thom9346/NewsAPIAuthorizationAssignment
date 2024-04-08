using Week14Security.Models;

namespace Week14Security.Services
{
    public interface IUserService
    {
        int GetCurrentUserId();
        string GenerateJwtToken(User user);
    }
}
