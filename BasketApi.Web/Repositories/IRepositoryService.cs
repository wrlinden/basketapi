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
        Task<BasketModel> RemoveItemsFromBasket(int basketId);
        Task<BasketModelItem> AddToBasketItem(int basketId, int basketItemId, int qty);

    }
}