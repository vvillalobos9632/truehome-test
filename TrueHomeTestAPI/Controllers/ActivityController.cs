using Common.DTOs.Activity;
using Common.Types.Roles;
using Framework.Authorization.Util;
using IServices.TrueHome;
using Microsoft.AspNetCore.Mvc;

namespace TrueHomeTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorized(SystemRoles.OWNER)]
    public class ActivityController : Controller
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService _activityService)
        {
            this._activityService = _activityService;
        }

        [HttpPost("GetActivitysPagination")]
        public IActionResult GetActivitysPagination([FromBody] ActivityFilterData activityFilterData)
        {
            return Ok(this._activityService.GetActivitysPagination(activityFilterData));
        }

        [HttpPost("GetInitActivityViewModel")]
        public IActionResult GetInitActivityViewModel([FromBody] int id)
        {
            return Ok(this._activityService.GetInitActivityViewModel(id));
        }

        [HttpPost("RegisterActivity")]
        public IActionResult RegisterActivity([FromBody] ActivityData activityData)
        {
            return Ok(this._activityService.RegisterActivity(activityData));
        }

        [HttpPost("UpdateActivity")]
        public IActionResult UpdateActivity([FromBody] ActivityData activityData)
        {
            return Ok(this._activityService.UpdateActivity(activityData));
        }

        [HttpPost("CancelActivity")]
        public IActionResult CancelActivity([FromBody] int id)
        {
            return Ok(this._activityService.CancelActivity(id));
        }

        [HttpPost("CompletelActivity")]
        public IActionResult CompletelActivity([FromBody] int id)
        {
            return Ok(this._activityService.CompletelActivity(id));
        }
    }
}