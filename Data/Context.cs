using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.Data
{
    public class Context : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Post> Posts { get; set; }
        
        public Context(DbContextOptions<Context> options) : base(options) { }
    }
}