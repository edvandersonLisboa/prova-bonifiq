using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class ProductService
	{
		TestDbContext _ctx;

		public ProductService(TestDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<ProductList>  ListProducts(int page)
		{
			var take = 2;
			var products =_ctx.Products;
			var countTotal =products.Count();
            
            var result = await _ctx.Products
				.Skip(page * take)
				.Take(take).ToListAsync();

            bool hasNext = page * take < countTotal;
            return new ProductList()
            {
                Products = result.ToList(),
                TotalCount = countTotal,
                HasNext = hasNext
            };
        }

	}
}
