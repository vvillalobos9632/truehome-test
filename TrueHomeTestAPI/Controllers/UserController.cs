using Common.DTOs.Users;
using Common.Types.Roles;
using Framework.Authorization.Util;
using IServices.TrueHome;
using Microsoft.AspNetCore.Mvc;

namespace TrueHomeTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorized(SystemRoles.OWNER)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("GetInitUserViewModel")]
        public IActionResult GetInitUserViewModel([FromBody] int idUsuario)
        {
            return Ok(_userService.GetInitUserViewModel(idUsuario));
        }

        [HttpPost("RegisterUser")]
        public IActionResult RegisterUser([FromBody] UsuariosData usuariosData)
        {
            return Ok(_userService.RegisterUser(usuariosData));
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateUsuarios([FromBody] UsuariosData usuariosData)
        {
            return Ok(_userService.UpdateUser(usuariosData));
        }

        [HttpGet("GetUsersPagination")]
        public IActionResult GetUsersPagination()
        {
            return Ok(_userService.GetUsersPagination());
        }
    }
}
