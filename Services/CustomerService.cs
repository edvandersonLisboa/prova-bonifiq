using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Shared.SystemDate.Interfaces;
using System.Threading.Tasks;

namespace ProvaPub.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        public CustomerService(IOrderRepository orderRepository, ICustomerRepository customerRepository,
             IDateTimeProvider dateTimeProvider)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<CustomerList> ListCustomers(int page, int pageSize)
        {

            (var customers, var total, var hasNext) = await _customerRepository.GetAllPagedAsync(page, pageSize);
            return new CustomerList() { HasNext = hasNext, TotalCount = total, Customers = customers.ToList() };
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) 
                throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) 
                throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null) 
                throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = DateTime.UtcNow.AddMonths(-1);
            var ordersInThisMonth = 
                await _orderRepository.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = 
                await _customerRepository.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            //Business Rule: A customer can purchases only during business hours and working days
            var now = _dateTimeProvider.UtcNow;
            if (now.Hour < 8 || now.Hour > 18 || now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
                return false;

            return true;
        }

    }
}
