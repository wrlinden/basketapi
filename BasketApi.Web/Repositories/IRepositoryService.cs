using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Models;

namespace BasketApi.Repositories
{
    public interface IRepositoryService
    {
        Task<BasketModel> CreateBasket(BasketModel basketModel);
    }

    public class InMemoryRepositoryService : IRepositoryService
    {
        private static readonly IList<BasketModel> Baskets = new List<BasketModel>();

        public async Task<BasketModel> CreateBasket(BasketModel basketModel)
        {
            if (Baskets.Any(item => item.Id == basketModel.Id))
                throw new System.Exception($"Basket item ${basketModel.Id} already exists.");

            await Task.Run(() => Baskets.Add(basketModel));
            return basketModel;
        }

        
    }
}