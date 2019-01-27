using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BasketApi.Contracts;
using BasketApi.Models;
using BasketApi.Repositories;

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



    public class BasketService : IBasketService
    {
        private readonly IRepositoryService _repositoryService;

        public BasketService()
        {
            // ToDo Remove once tests and IOC are in place
            _repositoryService = new InMemoryRepositoryService();
        }

        public BasketService(IRepositoryService repositoryService)
        {
            _repositoryService = repositoryService;
        }
        //public BasketService(IRepositoryService repositoryService)
        //{
        // for IoC 
        //}
        public async Task<BasketContract> CreateBasketAsync(BasketContract basketContract)
        {
            // ToDo Asumes that IDs are generated from the outside and are unique
            var basketModel = MapBasketContractToModel(basketContract);
            var newBasket = await _repositoryService.CreateBasket(basketModel);
            return MapBasketModelToContract(newBasket);

        }

        public async Task<BasketContract> GetBasket(int id)
        {
            var basket = await _repositoryService.GetBasketFromId(id);
            var basketContract = MapBasketModelToContract(basket);

            return basketContract;
        }

        public async Task<BasketContractItem> AddItemToBasketAsync(int basketId, BasketContractItem basketContractItem)
        {
            var basketModelItem = MapBasketContractItemToModel(basketContractItem);

            var newBasketModelItem = await _repositoryService.AddItemToBasket(basketId, basketModelItem);
            return newBasketModelItem != null ? MapBasketModelItemToContract(newBasketModelItem) : null;
        }
        public async Task<bool> RemoveItemFromBasketAsync(int basketId, int basketItemId)
        {
            bool itemRemoved = await _repositoryService.RemoveItemFromBasket(basketId, basketItemId);
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