using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SystemGroup.Utilities
{
    public class JwtTokenGenerator
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private AuthOptions authParams;

        public JwtTokenGenerator( IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions;
            authParams = authOptions.Value;
        }

        public string GenerateAccessToken(IdentityUser user)
        {
            var securityKey = authParams.GetAccessSimmetricSecuriryKey();

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            };

            var token = GenerateToken(securityKey, authParams.AccessTokenLifetime, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var securityKey = authParams.GetRefreshSimmetricSecuriryKey();
            var token = GenerateToken(securityKey, authParams.RefreshTokenLifetime);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private JwtSecurityToken GenerateToken(SymmetricSecurityKey securityKey,int refreshTokenLifeTime, List<Claim> claims=null)
        {
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                expires: DateTime.Now.AddSeconds(refreshTokenLifeTime),
                signingCredentials: credentials);

            return token;
        }
    }
}
