using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemGroup.Data.Context;
using SystemGroup.Models;

namespace SystemGroup.Data.Repository
{
    public class PostRepository : Repository<Post>
    {
        public PostRepository(ApplicationDbContext db) : base(db)
        {
        }
    }
}
