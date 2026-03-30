using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public const string SystemUser = "System User";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            this.Database.SetCommandTimeout(TimeSpan.FromSeconds(120));
        }

        public DbSet<TEntity> AsEntity<TEntity>() where TEntity : BaseEntity => Set<TEntity>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        public void Clear()
        {
            this.ChangeTracker.Clear();
        }
    }
}
