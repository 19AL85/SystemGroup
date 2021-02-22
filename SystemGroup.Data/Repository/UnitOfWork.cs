using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemGroup.Data.Context;
using SystemGroup.Data.Repository.IRepository;
using SystemGroup.Models;

namespace SystemGroup.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Posts = new PostRepository(_db);
            Users = new UserRepository(_db);
            RefreshTokens = new RefreshTokenRepository(_db);
        }

        public IRepository<Post> Posts { get; private set; }
        public IRepository<IdentityUser> Users { get; private set; }
        public IRepository<RefreshToken> RefreshTokens { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
