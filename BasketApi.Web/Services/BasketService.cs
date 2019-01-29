using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasketApi.Contracts;
using BasketApi.Models;
using BasketApi.Repositories;

namespace BasketApi.Services
{
    public class BasketService : IBasketService
    {
        private readonly IRepositoryService _repositoryService;

        public BasketService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }

        public async Task<BasketContract> CreateBasketAsync(BasketContract basketContract)
        {
            // ToDo [Note] Assumes that IDs are generated from the outside and are unique
            var basketModel = MapBasketContractToModel(basketContract);
            var newBasket = await _repositoryService.CreateBasket(basketModel);
            if (newBasket != null) return MapBasketModelToContract(newBasket);

            // Asumes null means something went wrong, hiher up in the stack needs to deal with this
            return null;

        }

        public async Task<BasketContract> GetBasket(int id)
        {
            var basket = await _repositoryService.GetBasketFromId(id);

            if (basket == null) return null;

            return MapBasketModelToContract(basket);
        }

        public async Task<BasketContractItem> AddItemToBasketAsync(int basketId, BasketContractItem basketContractItem)
        {
            var basketModelItem = MapBasketContractItemToModel(basketContractItem);

            var newBasketModelItem = await _repositoryService.AddItemToBasket(basketId, basketModelItem);
            return newBasketModelItem != null ? MapBasketModelItemToContract(newBasketModelItem) : null;
        }
        public async Task<bool> RemoveItemFromBasketAsync(int basketId, int basketItemId)
        {
            var itemRemoved = await _repositoryService.RemoveItemFromBasket(basketId, basketItemId);
            return itemRemoved;
        }

        public async Task<BasketContract> RemoveBasketItemsAsync(int basketId)
        {
            var basketRemovedFrom = await _repositoryService.RemoveItemsFromBasket(basketId);
            if (basketRemovedFrom != null) return MapBasketModelToContract(basketRemovedFrom);
            return null;
        }

        public async Task<BasketContractItem> AddToBasketItemAsync(int basketId, int basketItemId, int qty)
        {
            var updatedBasketModelItem = await _repositoryService.AddToBasketItem(basketId, basketItemId, qty);
            return updatedBasketModelItem != null ? MapBasketModelItemToContract(updatedBasketModelItem) : null;
        }

        // Helper Methods
        private static BasketContractItem MapBasketModelItemToContract(BasketModelItem basketModelItem)
        {
            return new BasketContractItem
            {
                Id = basketModelItem.Id,
                Qty = basketModelItem.Qty,
                ContractItem = new ContractItem
                {
                    Description = basketModelItem.ModelItem.Description,
                    Id = basketModelItem.ModelItem.Id,
                    Price = basketModelItem.ModelItem.Price
                }
            };
        }

        private BasketModelItem MapBasketContractItemToModel(BasketContractItem basketContractItem)
        {
            return new BasketModelItem
            {
                Id = basketContractItem.Id,
                Qty = basketContractItem.Qty,
                ModelItem = new ModelItem
                {
                    Id = basketContractItem.ContractItem.Id,
                    Description = basketContractItem.ContractItem.Description,
                    Price = basketContractItem.ContractItem.Price
                }
            };
        }

        private static BasketModel MapBasketContractToModel(BasketContract basket)
        {
            var basketModel = new BasketModel
            {
                Id = basket.Id,
                Items = MapBasketContractItemsToModel(basket.Items)
            };
            return basketModel;
        }

        private static IList<BasketModelItem> MapBasketContractItemsToModel(IEnumerable<BasketContractItem> basketContractItems)
        {
            var basketModelItems = basketContractItems.Select(basketContractItem => new BasketModelItem
                {
                    Id = basketContractItem.Id,
                    ModelItem = new ModelItem
                    {
                        Description = basketContractItem.ContractItem.Description,
                        Id = basketContractItem.ContractItem.Id,
                        Price = basketContractItem.ContractItem.Price
                    },
                    Qty = basketContractItem.Qty
                })
                .ToList();

            return basketModelItems;
        }

        private static BasketContract MapBasketModelToContract(BasketModel basketModel)
        {
            var contract = new BasketContract
            {
                Id = basketModel.Id,
                Items = MapBasketModelItemsToContract(basketModel.Items)
            };
            return contract;
        }

        private static IList<BasketContractItem> MapBasketModelItemsToContract(IEnumerable<BasketModelItem> basketItems)
        {
            return basketItems.Select(basketItem => new BasketContractItem
                {
                    Id = basketItem.Id,
                    Qty = basketItem.Qty,
                    ContractItem = new Contracts.ContractItem
                    {
                        Description = basketItem.ModelItem.Description,
                        Id = basketItem.ModelItem.Id,
                        Price = basketItem.ModelItem.Price
                    }
                })
                .ToList();
        }
    }
}