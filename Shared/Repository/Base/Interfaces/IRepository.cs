using Microsoft.EntityFrameworkCore;
using ProvaPub.Models.Entities;
using ProvaPub.Shared.Entities;
using System.Linq.Expressions;

namespace ProvaPub.Shared.Repository.Base.Interfaces
{
    public interface IRepository <TEntity> where TEntity : Entity
    {
        Task<(ICollection<TEntity>, int, bool)> GetAllPagedAsync(int page, int pageSize);

        Task<TEntity> Insert(TEntity value);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> GetByIdAsync(int id);

    }
}
