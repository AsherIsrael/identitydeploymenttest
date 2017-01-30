using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TheWall.Models
{
    public class TestContext : IdentityDbContext<TestUser>
    {
        public TestContext(DbContextOptions<TestContext> options)
            : base(options)
        { }
        
        public DbSet<TestUser> users { get; set; }

        public DbSet<Message> messages { get; set; }

        public DbSet<Comment> comments { get; set; }

        public DbSet<Product> products { get; set; }

        public DbSet<Shoppingcart> Shoppingcarts { get; set; }
    }
}
