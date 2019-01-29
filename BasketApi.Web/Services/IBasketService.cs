using System.Threading.Tasks;
using BasketApi.Contracts;

namespace BasketApi.Services
{
    public interface IBasketService
    {
        Task<BasketContract> CreateBasketAsync(BasketContract basket);
        Task<BasketContract> GetBasket(int id);
        Task<BasketContractItem> AddItemToBasketAsync(int basketId, BasketContractItem basketContractItem);
        Task<bool> RemoveItemFromBasketAsync(int basketId, int basketItemId);
        Task<BasketContract> RemoveBasketItemsAsync(int basketId);
        Task<BasketContractItem> AddToBasketItemAsync(int basketId, int basketItemId, int qty);


    }
}