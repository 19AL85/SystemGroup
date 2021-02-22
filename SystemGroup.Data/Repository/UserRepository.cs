using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemGroup.Data.Context;

namespace SystemGroup.Data.Repository
{
    public class UserRepository : Repository<IdentityUser>
    {
        public UserRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
