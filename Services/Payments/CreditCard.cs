using ProvaPub.Shared.Enums;

namespace ProvaPub.Services.Payments
{
    public class CreditCard : IPaymentService
    {
        public decimal Pay(decimal paymentValue, int customerId)
        {
            return 0.5M * paymentValue;
        }
    }
}
