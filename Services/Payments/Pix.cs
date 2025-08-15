using ProvaPub.Shared.Enums;

namespace ProvaPub.Services.Payments
{
    public class Pix : IPaymentService
    {
   

        public decimal Pay(decimal paymentValue, int customerId)
        {
            return 0.1M * paymentValue;
        }
    }
}
