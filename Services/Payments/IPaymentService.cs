using ProvaPub.Shared.Enums;

namespace ProvaPub.Services.Payments
{
    public interface IPaymentService 
    {
        decimal Pay(decimal paymentValue, int customerId);
    }
}
