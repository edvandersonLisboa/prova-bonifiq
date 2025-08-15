using ProvaPub.Shared.SystemDate.Interfaces;

namespace ProvaPub.Shared.SystemDate
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
