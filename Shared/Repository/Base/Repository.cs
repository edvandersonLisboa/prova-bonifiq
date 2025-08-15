using Microsoft.EntityFrameworkCore;
using ProvaPub.Repository;
using ProvaPub.Shared.Entities;
using ProvaPub.Shared.Repository.Base.Interfaces;
using System.Linq.Expressions;

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

        public async Task<TEntity> Insert(TEntity value)
        {
            try
            {
                var entry = await _dbSet.AddAsync(value);
                await _context.SaveChangesAsync(); // Aqui o banco gera o ID
                return entry.Entity;
            }
            catch( Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public virtual async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var count = 0;
                count = await _dbSet
                    .CountAsync(predicate);
            return count;
        }


        public virtual async Task<int> CountAsync()
        {
            var count = 0;
            count = await _dbSet
                .CountAsync();
            return count;
        }
        public virtual async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            
            ICollection<TEntity> result = await _dbSet.Where(predicate).ToListAsync();

            return result;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            TEntity entity = await _dbSet.AsNoTracking()
                     .SingleOrDefaultAsync(p => p.Id == id);
       
            return entity;
        }

        public virtual async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var result = await _dbSet
                .Where(predicate)
                .FirstOrDefaultAsync();

            return result;
        }
    }
}
