using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToBeApi.Models;

namespace ToBeApi.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            //modelBuilder.ApplyConfiguration(new CategoriesConfiguration());
            //modelBuilder.ApplyConfiguration(new UsersConfiguration());
            //modelBuilder.ApplyConfiguration(new PostsConfiguration());

        }

        public DbSet<Category> Categories { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

    }
}
