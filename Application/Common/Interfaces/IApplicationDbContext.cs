using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DbSet<TEntity> AsEntity<TEntity>() where TEntity : BaseEntity;
        void Clear();
    }
}
