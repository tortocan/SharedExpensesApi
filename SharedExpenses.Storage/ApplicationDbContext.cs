using Microsoft.EntityFrameworkCore;
using SharedExpenses.Storage.Models;

namespace SharedExpenses.Storage
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
           base(options)
        {
        }

        public DbSet<ApplicationUser> User { get; set; }
        public DbSet<Expense> Expense { get; set; }
        public DbSet<ExpenseGroup> ExpenseGroup { get; set; }
        public DbSet<Payment> Payment { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FullName).IsRequired();
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).IsRequired();
                entity.Property(e => e.Description).IsRequired();
            });

            modelBuilder.Entity<Expense>(entity =>
            {
                entity.HasKey(e => new{ e.UserId,e.ExpenseGroupId, e.PaymentId});
            });

            modelBuilder.Entity<ExpenseGroup>(entity => {
                entity.HasKey(e=>e.Id);
            });
        }
    }
}