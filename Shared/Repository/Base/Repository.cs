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
    }
}
