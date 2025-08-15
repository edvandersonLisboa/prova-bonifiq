using ProvaPub.Services.Factories.Interfaces;
using ProvaPub.Services.Payments;

namespace ProvaPub.Services.Factories
{
    public class PaymentServiceFactory : IPaymentServiceFactory
    {
        private readonly Dictionary<string, IPaymentService> _paymentServices;

        public PaymentServiceFactory(IEnumerable<IPaymentService> paymentServices)
        {
          _paymentServices = paymentServices
          .ToDictionary(
              service => service.GetType().Name.ToLower(),
              service => service
          );
        }

        public IPaymentService GetPaymentMethod(string paymentMethod) 
        {
            if(_paymentServices.TryGetValue(paymentMethod.ToLower(), out var service))
            {
                return service;
            }

            throw new ArgumentException("Métod de pagamento invalido");
        }
    }
}
