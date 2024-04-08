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
        public IActionResult CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null)
            {
                return BadRequest();
            }

            var user = _converter.Convert(userDTO);
            user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);

            var newUser = _userRepository.Add(user);

            return CreatedAtRoute("GetUser", new { id = newUser.UserId }, _converter.Convert(newUser));
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
        public IEnumerable<UserDTO> GetAll()
        {
            var userDtoList = new List<UserDTO>();

            foreach (var user in _userRepository.GetAll())
            {
                var userDto = _converter.Convert(user);
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

    }
}
