using Common.DTOs.Users;
using IServices.TrueHome;
using Microsoft.AspNetCore.Mvc;

namespace TrueHomeTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginData userLoginData)
        {
            return Ok(_userService.Login(userLoginData));
        }

    }
}
