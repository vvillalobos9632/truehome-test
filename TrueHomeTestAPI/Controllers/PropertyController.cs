using Common.DTOs.Property;
using Common.Types.Roles;
using Framework.Authorization.Util;
using IServices.TrueHome;
using Microsoft.AspNetCore.Mvc;

namespace TrueHomeTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorized(SystemRoles.OWNER)]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService _propertyService)
        {
            this._propertyService = _propertyService;
        }

        [HttpGet("GetPropertysPagination")]
        public IActionResult GetPropertysPagination()
        {
            return Ok(this._propertyService.GetPropertysPagination());
        }

        [HttpPost("GetInitPropertyViewModel")]
        public IActionResult GetInitPropertyViewModel([FromBody] int id)
        {
            return Ok(this._propertyService.GetInitPropertyViewModel(id));
        }

        [HttpPost("RegisterProperty")]
        public IActionResult RegisterProperty([FromBody] PropertyData propertyData)
        {
            return Ok(this._propertyService.RegisterProperty(propertyData));
        }

        [HttpPost("UpdateProperty")]
        public IActionResult UpdateProperty([FromBody] PropertyData propertyData)
        {
            return Ok(this._propertyService.UpdateProperty(propertyData));
        }
    }
}