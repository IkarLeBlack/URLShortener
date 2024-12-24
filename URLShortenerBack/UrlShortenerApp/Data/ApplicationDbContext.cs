using Microsoft.EntityFrameworkCore;
using URL_Shortener.Models;

namespace URL_Shortener.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        
        public DbSet<UrlModel> Urls { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<UrlModel>()
                .HasIndex(u => u.ShortUrl)
                .IsUnique();

            
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Username)
                .IsUnique();

            
            modelBuilder.Entity<RoleModel>().HasData(
                new RoleModel { Id = 1, RoleName = "Admin" },
                new RoleModel { Id = 2, RoleName = "User" }
            );
        }
    }
}
