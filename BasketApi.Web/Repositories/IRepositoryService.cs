using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Models;

namespace BasketApi.Repositories
{
    public interface IRepositoryService
    {
        Task<BasketModel> CreateBasket(BasketModel basketModel);
        Task<BasketModel> GetBasketFromId(int id);
        Task<BasketModelItem> AddItemToBasket(int basketId, BasketModelItem basketModelItem);
        Task<bool> RemoveItemFromBasket(int basketId, int basketItemId);

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

        public async Task<BasketModel> GetBasketFromId(int id)
        {
            var getBasketTask = Task.Run(() => Baskets.FirstOrDefault(x => x.Id == id));
            return await getBasketTask;
        }


        public async Task<BasketModelItem> AddItemToBasket(int basketId, BasketModelItem basketModelItem)
        {

            var basketToAddTo = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            var basketItemToAddTo = basketToAddTo?.Items.FirstOrDefault(item => item.Id == basketModelItem.Id);

            // Only add to an existing basket; only add a basket item if it does not already exist
            if (basketToAddTo == null || basketItemToAddTo != null) return null;


            await Task.Run(() =>
            {
                basketToAddTo.Items.Add(basketModelItem);
            });
            
            return basketModelItem;
        }

        public async Task<bool> RemoveItemFromBasket(int basketId, int basketItemId)
        {
            var basketToRemoveFrom = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            var basketItemToRemove = basketToRemoveFrom?.Items.SingleOrDefault(item => item.Id == basketItemId);

            var removeFromBasketTask = Task.Run(() => basketToRemoveFrom != null && basketToRemoveFrom.Items.Remove(basketItemToRemove));

            if (basketItemToRemove != null) return await removeFromBasketTask;

            return false;
        }
    }
}