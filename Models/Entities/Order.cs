using ProvaPub.Shared.Entities;

namespace ProvaPub.Models.Entities
{
	public class Order : Entity
	{

		public decimal Value { get; set; }
		public int CustomerId { get; set; }
		public DateTime OrderDate { get; set; }
		public Customer Customer { get; set; }

		public void BrazilTimeZone()
		{
			OrderDate = TimeZoneInfo.ConvertTimeFromUtc(OrderDate, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));
        }
	}
}
