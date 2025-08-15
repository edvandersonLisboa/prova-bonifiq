using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;

namespace ProvaPub.Services
{
	public class ProductService
	{
		TestDbContext _ctx;
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository produtoRepository)
		{
			_productRepository = produtoRepository;
		}
		
		public async Task<ProductList>  ListProducts(int page, int pageSize)
		{
			

			(var products, var total, var hasNext) = await _productRepository.GetAllPagedAsync(page, pageSize);
            return new ProductList()
            {
                Products = products.ToList(),
                TotalCount = total,
                HasNext = hasNext
            };
        }

	}
}
