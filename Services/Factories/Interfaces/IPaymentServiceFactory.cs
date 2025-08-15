using ProvaPub.Services.Payments;

namespace ProvaPub.Services.Factories.Interfaces
{
    public interface IPaymentServiceFactory
    {
        IPaymentService GetPaymentMethod(string paymentMethod);
    }
}
