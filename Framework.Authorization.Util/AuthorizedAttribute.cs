using Common.Extensions.Utils;
using Common.Types.Roles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;

namespace Framework.Authorization.Util
{
    public class AuthorizedAttribute : AuthorizeAttribute
    {
        public AuthorizedAttribute(params SystemRoles[] roles) : base()
        {
            if (roles.Contains(SystemRoles.ALL))
                roles = EnumExtensions.GetFullEnumList<SystemRoles>().ToArray();

            var rolesKeys = EnumExtensions.GetListStringValuesForEnum(roles.ToList());

            Roles = String.Join(",", rolesKeys.ToArray());
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }

    }
}
