using Microsoft.EntityFrameworkCore;
using SharedExpensesApi.Models;

namespace SharedExpensesApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
           base(options)
        {

        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Expences> Expence { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired();
            });
        }
    }
}