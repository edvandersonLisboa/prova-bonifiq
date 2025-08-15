using ProvaPub.Models.Entities;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Shared.Repository.Base;

namespace ProvaPub.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(TestDbContext context) : base(context)
        {
        }
    }
}
