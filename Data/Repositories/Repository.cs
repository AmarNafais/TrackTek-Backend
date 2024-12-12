using Data.Entities;
using Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class Repository : DbContext
    {
        public Repository(DbContextOptions<Repository> options) : base(options)
        {
        }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Customer> Customers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (UserRole)Enum.Parse(typeof(UserRole), v));
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.SetCreatedTime();
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.SetUpdatedTime();
                }
            }

            return base.SaveChanges();
        }
    }
}
