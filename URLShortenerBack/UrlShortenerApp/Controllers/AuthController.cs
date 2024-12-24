using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using URL_Shortener.Services;

namespace URL_Shortener.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserLoginDto loginDto)
        {
            if (string.IsNullOrEmpty(loginDto.Username) || string.IsNullOrEmpty(loginDto.PasswordHash))
            {
                return BadRequest("Username and Password hash are required.");
            }
            
            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);
            if (user == null)
            {

                await _userService.CreateNewUserAsync(loginDto.Username, loginDto.PasswordHash);
                user = await _userService.GetUserByCredentialsAsync(loginDto.Username, loginDto.PasswordHash);
            }


            if (user == null || user.PasswordHash != loginDto.PasswordHash)
            {
                return BadRequest("Invalid password.");
            }


            return Ok(new
            {
                user.Id,
                user.Username,
                Role = user.Role?.RoleName ?? "User"
            });
        }
    }
}