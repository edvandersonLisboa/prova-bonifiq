using ProvaPub.Models.Entities;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Shared.Repository.Base;

namespace ProvaPub.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(TestDbContext context) : base(context)
        {
        }
    }
}
