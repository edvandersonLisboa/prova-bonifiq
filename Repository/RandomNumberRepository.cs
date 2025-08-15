using ProvaPub.Models.Entities;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Shared.Repository.Base;

namespace ProvaPub.Repository
{
    public class RandomNumberRepository : Repository<RandomNumber>, IRandomNumberRepository
    {
        public RandomNumberRepository(TestDbContext context) : base(context)
        {
        }
    }
}
