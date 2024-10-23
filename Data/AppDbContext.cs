using Microsoft.EntityFrameworkCore;
using BitRent.Models;

namespace BitRent.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<BitTransaction> BitTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var fk in modelBuilder.Model.GetEntityTypes().SelectMany(s => s.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<AppUser>().ToTable("AppUser").HasDiscriminator(i=>i.Discriminator);
            
            modelBuilder.Entity<Property>().ToTable("Property");
            modelBuilder.Entity<BitTransaction>().ToTable("BitTansaction");

            
        }
    }
}
