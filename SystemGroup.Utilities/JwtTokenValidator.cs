using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemGroup.Utilities
{
    public class JwtTokenValidator
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private AuthOptions authParams;

        public JwtTokenValidator(IOptions<AuthOptions> authOptions)
        {
            _authOptions = authOptions;
            authParams = authOptions.Value;
        }

        public bool IsRefreshTokenValid(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = authParams.Issuer,

                ValidateAudience = true,
                ValidAudience = authParams.Audience,

                ValidateLifetime = true,

                IssuerSigningKey = authParams.GetRefreshSimmetricSecuriryKey(),
                ValidateIssuerSigningKey = true,

                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
