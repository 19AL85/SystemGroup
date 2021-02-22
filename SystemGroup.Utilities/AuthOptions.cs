using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemGroup.Utilities
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessSecret { get; set; } //sec
        public string RefreshSecret { get; set; } //sec
        public int AccessTokenLifetime { get; set; }
        public int RefreshTokenLifetime { get; set; }

        public SymmetricSecurityKey GetAccessSimmetricSecuriryKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AccessSecret));
        }

        public SymmetricSecurityKey GetRefreshSimmetricSecuriryKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(RefreshSecret));
        }
    }
}
