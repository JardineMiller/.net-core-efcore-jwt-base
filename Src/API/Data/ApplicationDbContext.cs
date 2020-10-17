using System;
using System.Threading;
using System.Threading.Tasks;
using API.Data.Models;
using API.Data.Models.Base;
using API.Infrastructure.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private readonly ICurrentUserService currentUserService;

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            this.currentUserService = currentUserService;
        }

        public DbSet<ExampleEntity> ExampleEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(builder);
        }

        #region Audit Info

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditInformation();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void ApplyAuditInformation()
        {
            var auditEntries = this.ChangeTracker.Entries<IAuditEntity>();
            var userName = this.currentUserService.GetUserName();

            foreach (var entityEntry in auditEntries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        entityEntry.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        entityEntry.Entity.CreatedBy = userName;
                        break;

                    case EntityState.Modified:
                        entityEntry.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                        entityEntry.Entity.ModifiedBy = userName;
                        break;

                    case EntityState.Deleted:
                        if (entityEntry.Entity is IDeletableEntity deletableEntity)
                        {
                            deletableEntity.DeletedOn = DateTimeOffset.UtcNow;
                            deletableEntity.DeletedBy = userName;
                            deletableEntity.IsDeleted = true;

                            entityEntry.State = EntityState.Modified;
                        }

                        break;
                }
            }
        }

        #endregion
    }
}
