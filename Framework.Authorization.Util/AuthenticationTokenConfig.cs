using Common.DTOs.Users;
using Common.Extensions.Utils;
using Common.Types.Roles;
using Microsoft.IdentityModel.Tokens;
using Presentation.CommonSettings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Framework.Authorization.Util
{
    public class AuthenticationTokenConfig
    {
        public static string GenerateJSONWebToken(UserLoginData userLoginData, List<SystemRoles> rolesPermitidos, out DateTime expireDate)
        {
            if (rolesPermitidos.Contains(SystemRoles.OWNER))
                rolesPermitidos = EnumExtensions.GetFullEnumList<SystemRoles>();

            var tokenValidationParameters = CommonSettings.TokenValidationParameters;
            var rolesPermitidosString = EnumExtensions.GetListStringValuesForEnum(rolesPermitidos);
            var securekey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValidationParameters.SecurityKey));
            var credentials = new SigningCredentials(securekey, SecurityAlgorithms.HmacSha256);
            var password = userLoginData.Contrasena;
            userLoginData.Contrasena = null;
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName,JsonExtensions.SerializeToJson(userLoginData)),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            userLoginData.Contrasena = password;

            claims.AddRange(rolesPermitidosString.Select(s => new Claim(ClaimTypes.Role, s)).ToList());

            expireDate = DateTime.Now.AddMinutes(tokenValidationParameters.MinExpiresToken);
            var token = new JwtSecurityToken(
                issuer: tokenValidationParameters.ValidIssuer,
                audience: tokenValidationParameters.ValidAudience,
                claims: claims.ToArray(),
                expires: expireDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
