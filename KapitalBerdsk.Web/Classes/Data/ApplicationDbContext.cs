using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BuildingObject> BuildingObjects { get; set; }
        public DbSet<PdSection> PdSections { get; set; }
        public DbSet<FundsFlow> FundsFlows { get; set; }
        public DbSet<Organization> Organizations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Employee>().HasIndex(u => u.FullName).IsUnique();
            builder.Entity<BuildingObject>().HasIndex(u => u.Name).IsUnique();
            builder.Entity<Organization>().HasIndex(u => u.Name).IsUnique();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            AddTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IAuditable &&
                (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((IAuditable)entity.Entity).DateCreated = DateTime.UtcNow;
                    ((IAuditable)entity.Entity).DateUpdated = ((IAuditable)entity.Entity).DateCreated;
                }
                else if (entity.State == EntityState.Modified)
                {
                    ((IAuditable)entity.Entity).DateUpdated = DateTime.UtcNow;
                }
            }
        }
    }
}
