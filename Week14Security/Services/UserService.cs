using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Week14Security.Models;
using Week14Security.Repositories;

namespace Week14Security.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        public int GetCurrentUserId()
        {
            var userIdStr = _httpContextAccessor.HttpContext.User.Claims
             .FirstOrDefault(c => c.Type == "id")?.Value;

            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new Exception("User is not authenticated.");
            }

            if (int.TryParse(userIdStr, out int userId))
            {
                return userId;
            }

            throw new Exception("User is not authenticated or ID is not in a valid format.");
        }
        public string GenerateJwtToken(User user)
        {
            var role = _userRepository.GetUserRole(user.UserId);

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _configuration.GetValue<string>("Jwt:Key");
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new InvalidOperationException("JWT signing key is not set in configuration.");
            }

            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("id", user.UserId.ToString()),
                new Claim(ClaimTypes.Role, role)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
