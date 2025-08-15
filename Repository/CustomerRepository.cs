using ProvaPub.Models.Entities;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Shared.Repository.Base;

namespace ProvaPub.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TestDbContext context) : base(context)
        {
        }
    }
}
