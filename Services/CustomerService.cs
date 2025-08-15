using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;
using System.Threading.Tasks;

namespace ProvaPub.Services
{
    public class CustomerService
    {
        TestDbContext _ctx;
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(TestDbContext ctx, ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public CustomerService()
        {
            
        }
        public async Task<CustomerList> ListCustomers(int page, int pageSize)
        {

            (var customers, var total, var hasNext) = await _customerRepository.GetAllPagedAsync(page, pageSize);
            return new CustomerList() { HasNext = hasNext, TotalCount = total, Customers = customers.ToList() };
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            //Business Rule: A customer can purchases only during business hours and working days
            if (DateTime.UtcNow.Hour < 8 || DateTime.UtcNow.Hour > 18 || DateTime.UtcNow.DayOfWeek == DayOfWeek.Saturday || DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                return false;


            return true;
        }

    }
}
