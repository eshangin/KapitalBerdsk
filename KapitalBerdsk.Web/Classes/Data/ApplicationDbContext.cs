using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using KapitalBerdsk.Web.Classes.Data.Enums;
using KapitalBerdsk.Web.Classes.Data.Interfaces;
using KapitalBerdsk.Web.Classes.Models;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KapitalBerdsk.Web.Classes.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IDataProtectionKeyContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<BuildingObject> BuildingObjects { get; set; }
        public DbSet<PdSection> PdSections { get; set; }
        public DbSet<FundsFlow> FundsFlows { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<EmployeePayroll> EmployeePayrolls { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
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

            builder.Entity<FundsFlow>().Property(u => u.OutgoType).HasDefaultValue(OutgoType.Regular);
            builder.Entity<BuildingObject>().Property(u => u.Status).HasDefaultValue(BuildingObjectStatus.Active);

            builder.Entity<ApplicationUser>().HasOne(u => u.CreatedBy).WithMany(u => u.CreatedByMe);
            builder.Entity<ApplicationUser>().HasOne(u => u.ModifiedBy).WithMany(u => u.ModifiedByMe);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            FillAuditableFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            FillAuditableFields();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private string GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return null;
            }

            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private void FillAuditableFields()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is IAuditableWithDates &&
                (x.State == EntityState.Added || x.State == EntityState.Modified));

            if (entities.Count() > 0)
            {
                string currentUserId = GetCurrentUserId();
                foreach (var entity in entities)
                {
                    var auditable = ((IAuditableWithDates)entity.Entity);
                    if (entity.State == EntityState.Added)
                    {
                        auditable.DateCreated = DateTime.UtcNow;
                        auditable.DateUpdated = auditable.DateCreated;
                        if (auditable is IAuditable)
                        {
                            ((IAuditable)auditable).CreatedById = currentUserId;
                            ((IAuditable)auditable).ModifiedById = currentUserId;
                        }
                    }
                    else if (entity.State == EntityState.Modified)
                    {
                        auditable.DateUpdated = DateTime.UtcNow;
                        if (auditable is IAuditable)
                        {
                            ((IAuditable)auditable).ModifiedById = currentUserId;
                        }
                    }
                }
            }
        }
    }
}
