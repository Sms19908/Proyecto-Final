using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SC_701_PAW_Lunes.Models;

namespace SC_701_PAW_Lunes.Data
{
    public class PAWDbContext : IdentityDbContext<User>
    {
        public PAWDbContext(DbContextOptions<PAWDbContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<Category> Category { get; set;}
        public DbSet<User> Users { get; set; }
    }
}
