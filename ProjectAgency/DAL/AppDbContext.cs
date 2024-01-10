using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectAgency.Models;

namespace ProjectAgency.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
    }
}
