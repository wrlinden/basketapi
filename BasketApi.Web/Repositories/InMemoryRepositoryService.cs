using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Models;

namespace BasketApi.Repositories
{
    // ToDo  [Note] async / Task.Run used to simulate async repository access. 
    public class InMemoryRepositoryService : IRepositoryService
    {

        private static readonly IList<BasketModel> Baskets = new List<BasketModel>();

        public async Task<BasketModel> CreateBasket(BasketModel basketModel)
        {
            if (Baskets.Any(item => item.Id == basketModel.Id)) return null;
            Baskets.Add(basketModel);
            return await Task.FromResult<BasketModel>(basketModel);
        }

        public async Task<BasketModel> GetBasketFromId(int id)
        {
            return await Task.FromResult<BasketModel>(Baskets.FirstOrDefault(x => x.Id == id));
        }


        public async Task<BasketModelItem> AddItemToBasket(int basketId, BasketModelItem basketModelItem)
        {

            var basketToAddTo = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            var basketItemToAddTo = basketToAddTo?.Items.FirstOrDefault(item => item.Id == basketModelItem.Id);

            // Only add to an existing basket; only add a basket item if it does not already exist
            if (basketToAddTo == null || basketItemToAddTo != null) return null;

            basketToAddTo.Items.Add(basketModelItem);
            return await Task.FromResult(basketModelItem);
           
        }

        public async Task<bool> RemoveItemFromBasket(int basketId, int basketItemId)
        {
            var basketToRemoveFrom = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            var basketItemToRemove = basketToRemoveFrom?.Items.SingleOrDefault(item => item.Id == basketItemId);
            if (basketItemToRemove == null) return false;
            return await Task.FromResult(basketToRemoveFrom.Items.Remove(basketItemToRemove));
        }

        public async Task<BasketModel> RemoveItemsFromBasket(int basketId)
        {
            var basketToRemoveItemsFrom = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            if (basketToRemoveItemsFrom == null) return null;
            basketToRemoveItemsFrom.Items.Clear();

            return await Task.FromResult(Baskets.FirstOrDefault(basket => basket.Id == basketId));
        }

        public async Task<BasketModelItem> AddToBasketItem(int basketId, int basketItemId, int qty)
        {

            var basketToAddTo = Baskets.FirstOrDefault(basket => basket.Id == basketId);
            var basketItemToAddTo = basketToAddTo?.Items.FirstOrDefault(item => item.Id == basketItemId);


            if (basketToAddTo == null || basketItemToAddTo == null) return null;

            // ToDo Assumption, qty only goes to 0, could also check for zero vals not to hit "real" repo
            basketItemToAddTo.Qty += qty;
            if (basketItemToAddTo.Qty < 1) basketItemToAddTo.Qty = 0;
            var basketAddedTo = Baskets.FirstOrDefault(basket => basket.Id == basketId);

            return await Task.FromResult( basketAddedTo?.Items.FirstOrDefault(item => item.Id == basketItemId));
           
        }
    }
}