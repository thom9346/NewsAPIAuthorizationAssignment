using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Week14Security.Data;
using Week14Security.DTOs;
using Week14Security.Models;
using Week14Security.Models.Converters;
using Week14Security.Repositories;
using Week14Security.Services;
using Week14Security.Utility;

namespace Week14Security.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConverter<User, UserDTO> _converter;
        private readonly IUserService _userService;

        public UserController(IUserRepository repository, IConverter<User, UserDTO> converter, IUserService userService)
        {
            _userRepository = repository;
            _converter = converter;
            _userService = userService;

        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthRequestDTO authRequest)
        {
            if (authRequest == null)
            {
                return BadRequest("Invalid request");
            }

            var user = _userRepository.FindByUsername(authRequest.Username);

            if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, authRequest.PasswordHash))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new { user.UserId, user.Username, Token = token });
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreationDTO userCreationDto)
        {
            if (userCreationDto == null)
            {
                return BadRequest();
            }

            User user = new User
            {
                Username = userCreationDto.Username,
                Role = userCreationDto.Role,
                PasswordHash = PasswordHasher.HashPassword(userCreationDto.Password)
            };

            var newUser = _userRepository.Add(user);
            var userDto = _converter.Convert(newUser);

            return CreatedAtRoute("GetUser", new { id = newUser.UserId }, userDto);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult Get(int id)
        {
            var item = _userRepository.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            var userDto = _converter.Convert(item);
            return new ObjectResult(userDto);
        }

        [HttpGet]
        public IEnumerable<UserDTO> GetAll() => _userRepository.GetAll().Select(_converter.Convert);

    }
}
