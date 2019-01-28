using System;
using System.Net.Http;
using System.Threading.Tasks;
using BasketApi.Contracts;


namespace BasketApi.Client
{

    public class BasketClient
    {

        private readonly string _baseAddress; 
        private readonly HttpClient _client;

        public BasketClient(string serverUrl)
        {
            _baseAddress = serverUrl + "api/basket/";
            _client = new HttpClient();
        }

        public async Task<BasketContract> CreateBasket(BasketContract basketContract)
        {
            var response = await _client.PostAsJsonAsync(_baseAddress, basketContract);
            ThrowExceptionIfUnsuccessfullStatusCode(response);
            return await response.Content.ReadAsAsync<BasketContract>();
        }

        public async Task<BasketContract> GetBasketById(int id)
        {
            var response = await _client.GetAsync(_baseAddress + $"{id}");
            ThrowExceptionIfUnsuccessfullStatusCode(response);
            return await response.Content.ReadAsAsync<BasketContract>();
        }

        public async Task<BasketContractItem> AddBasketItem(int basketContractId, BasketContractItem basketContractItem)
        {
            var response = await _client.PostAsJsonAsync(_baseAddress + $"{basketContractId}/item", basketContractItem);

            ThrowExceptionIfUnsuccessfullStatusCode(response);

            var newBasketContractItem = await response.Content.ReadAsAsync<BasketContractItem>();
            return newBasketContractItem;
        }

        public async Task<bool> RemoveFromBasket(int basketContractId, int basketContractItemId)
        {
            var response = await _client.DeleteAsync(_baseAddress + $"{basketContractId}/item/{basketContractItemId}");

            ThrowExceptionIfUnsuccessfullStatusCode(response);

            return true;

        }

        public async Task RemoveBasketItems(int basketContractId)
        {
            var response = await _client.DeleteAsync(_baseAddress + $"{basketContractId}/items/");
            ThrowExceptionIfUnsuccessfullStatusCode(response);
        }


        public async Task<BasketContractItem> AddToBasketItem(int basketContractId, int basketContractItemId, int qty = 1)
        {
            return await GetQtyUpdatedBasketContractItem(basketContractId, basketContractItemId, qty);
        }

        public async Task<BasketContractItem> RemoveFromBasketItem(int basketContractId, int basketContractItemId, int qty = 1)
        {
            return await GetQtyUpdatedBasketContractItem(basketContractId, basketContractItemId, 0 - qty);
        }


        // Helper Methods

        private async Task<BasketContractItem> GetQtyUpdatedBasketContractItem(int basketContractId, int basketContractItemId, int qty)
        {
            var response = await _client.PutAsJsonAsync(_baseAddress + $"{basketContractId}/item/{basketContractItemId}", qty);

            ThrowExceptionIfUnsuccessfullStatusCode(response);

            var updatedBasketContractItem = await response.Content.ReadAsAsync<BasketContractItem>();
            return updatedBasketContractItem;
        }


        private static void ThrowExceptionIfUnsuccessfullStatusCode(HttpResponseMessage response)
        {

            if (!response.IsSuccessStatusCode)
                throw new Exception("Problem with request to Add basket Item endpoint with Response : " +
                                    $"{response.StatusCode}\n " +
                                    $"{response.RequestMessage}\n " +
                                    $"{response.ReasonPhrase}");
        }

    }
}
