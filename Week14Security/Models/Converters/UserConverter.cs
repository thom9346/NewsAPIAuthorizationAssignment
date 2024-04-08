namespace Week14Security.Models.Converters
{
    public class UserConverter : IConverter<User, UserDTO>
    {
        public User Convert(UserDTO model)
        {
            return new User
            {
                UserId = model.UserId,
                Username = model.Username,
                PasswordHash = model.PasswordHash,
                Role = model.Role,
            };
        }

        public UserDTO Convert(User model)
        {
            return new UserDTO
            {
                UserId = model.UserId,
                Username = model.Username,
                PasswordHash = model.PasswordHash,
                Role = model.Role,
            };
        }
    }
}
