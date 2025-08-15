using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Models.Entities;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
	public class RandomService
	{
        private  readonly Random _rnd;
        int seed;
        TestDbContext _ctx;
        int maxItem = 100;
        int minItem = 1;
		public RandomService(Random rnd, TestDbContext ctx)
        {
            

            
            var bytes = Guid.NewGuid().ToByteArray();
            seed = BitConverter.ToInt32(bytes, 0);
            _rnd = rnd;
            _ctx = ctx;
        }
        public async Task<string> GetRandom()
		{

            //var number =  new Random(seed).Next(100);
            var number = _rnd.Next(1, maxItem + 1);
            var nuber =await _ctx.Numbers.AsNoTracking().FirstOrDefaultAsync(p => p.Number == number);
            var countItem = await _ctx.Numbers.AsNoTracking().CountAsync();

            if (countItem == maxItem)
            {
                return $"Todos os numeros de {minItem} a {maxItem} já foram cadastrados";
            }

            if (nuber == null)
            {
                
                _ctx.Numbers.Add(new RandomNumber() { Number = number });
                _ctx.SaveChanges();
            }
            else
            {
                return $"valor já existe cadastrado na base de dados {number}";
            }

            return number.ToString();
		}

	}
}
