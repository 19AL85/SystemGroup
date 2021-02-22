using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemGroup.Data.Context;
using SystemGroup.Models;

namespace SystemGroup.Data.Repository
{
    public class RefreshTokenRepository : Repository<RefreshToken>
    {
        public RefreshTokenRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
