using ProvaPub.Models.Entities;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;
using ProvaPub.Services.Factories;
using ProvaPub.Services.Factories.Interfaces;
using ProvaPub.Services.Interfaces;

namespace ProvaPub.Services
{
	public class OrderService : IOrderService
	{
        TestDbContext _ctx;
        private readonly IPaymentServiceFactory _paymentFactory;
        private readonly IOrderRepository _orderRepository;
        public OrderService( IPaymentServiceFactory paymentFactory, IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
			_paymentFactory = paymentFactory;
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
		{
            var paymentService = _paymentFactory.GetPaymentMethod(paymentMethod);
            var value = paymentService.Pay(paymentValue, customerId);

            return await InsertOrder(new Order()
            {
                CustomerId = customerId,
                Value = paymentValue + value,
                OrderDate = DateTime.UtcNow
            });


		}

		public async Task<Order> InsertOrder(Order order)
        {
            order = await _orderRepository.Insert(order);
            order.BrazilTimeZone();
            //Insere pedido no banco de dados
            return order;
        }
	}
}
