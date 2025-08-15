using ProvaPub.Shared.Entities;

namespace ProvaPub.Models.Entities
{
	public class Customer: Entity
	{
	
		public string Name { get; set; }
		public ICollection<Order> Orders { get; set; }
	}
}
