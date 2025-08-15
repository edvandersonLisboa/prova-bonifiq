using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Models.Entities;
using ProvaPub.Repository;
using ProvaPub.Repository.Interfaces;

namespace ProvaPub.Services
{
	public class RandomService
	{
        private  readonly Random _rnd;
        private readonly IRandomNumberRepository _randoNumberRepository;
        int maxItem = 100;
        int minItem = 1;
        public RandomService(Random rnd, IRandomNumberRepository randoNumberRepository)
        {


            _rnd = rnd;
            var bytes = Guid.NewGuid().ToByteArray();
            _randoNumberRepository = randoNumberRepository;
        }
        public async Task<string> GetRandom()
		{

            //var number =  new Random(seed).Next(100);
            var number = _rnd.Next(1, maxItem + 1);
            var nuber =await _randoNumberRepository.SingleOrDefaultAsync(p => p.Number == number);
            var countItem = await _randoNumberRepository.CountAsync();

            if (countItem == maxItem)
            {
                return $"Todos os numeros de {minItem} a {maxItem} já foram cadastrados";
            }

            if (nuber == null)
            {               
                await _randoNumberRepository.Insert( new RandomNumber() { Number = number });
            }
            else
            {
                return $"valor já existe cadastrado na base de dados {number}";
            }

            return number.ToString();
		}

	}
}
