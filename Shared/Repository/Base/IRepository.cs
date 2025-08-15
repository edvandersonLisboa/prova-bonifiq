using ProvaPub.Models.Entities;
using ProvaPub.Shared.Entities;

namespace ProvaPub.Shared.Repository.Base
{
    public interface IRepository <TEntity> where TEntity : Entity
    {
        Task<(ICollection<TEntity>, int, bool)> GetAllPagedAsync(int page, int pageSize);

        Task<TEntity> Insert(TEntity value);
    }
}
