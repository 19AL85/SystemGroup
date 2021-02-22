using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemGroup.Models;

namespace SystemGroup.Data.Repository.IRepository
{
    public interface IUnitOfWork:IDisposable
    {
        public IRepository<Post> Posts{ get; }
        public IRepository<IdentityUser> Users{ get;}
        public IRepository<RefreshToken> RefreshTokens{ get;}

        public void Save();
    }
}
