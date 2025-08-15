using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Shared.Entities;

namespace ProvaPub.Shared.Repository.Base
{
    public class Repository <TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly TestDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(TestDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<(ICollection<TEntity>,int, bool)> GetAllPagedAsync(int page, int pageSize = 5)
        {
            var countTotal = _dbSet.Count();
            var result = new List<TEntity>();
                result = await _dbSet
                    .AsNoTracking()
                    .Skip((page -1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            bool hasNext = (page * pageSize) < countTotal;

            return (result,countTotal,hasNext);
        }

    }
}
