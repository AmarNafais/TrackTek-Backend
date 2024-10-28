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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserRole to be stored as a string in the database
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(
                    v => v.ToString(), // Convert enum to string for storage
                    v => (UserRole)Enum.Parse(typeof(UserRole), v)); // Convert back to enum
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
