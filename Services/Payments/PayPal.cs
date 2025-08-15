using ProvaPub.Shared.Enums;

namespace ProvaPub.Services.Payments
{
    public class PayPal : IPaymentService
    {
        public decimal Pay(decimal paymentValue, int customerId)
        {
            return 0.2M * paymentValue;
        }
    }
}
