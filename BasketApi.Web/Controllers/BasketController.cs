using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BasketApi.Contracts;
using BasketApi.Services;

namespace BasketApi.Controllers
{
    public class BasketController : ApiController
    {

        private readonly IBasketService _basketService;
        public BasketController()
        {
            _basketService = new BasketService();
        }
        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // Create a new Basket
        // POST: api/Basket
        public async Task<BasketContract> Post([FromBody] BasketContract basket)
        {
            return await _basketService.CreateBasketAsync(basket);
        }

        
        [HttpGet]
        // Get basket by Id
        // GET: api/Basket/{id}
        public async Task<BasketContract> Get([FromUri] int id)
        {
            var basket = await _basketService.GetBasket(id);
            return basket;
        }

        
        [HttpPost]
        [Route("api/basket/{id}/item")]
        // Add item to Basket
        // POST: api/Basket/{id}/item
        public async Task<BasketContractItem> Post([FromUri] int id, [FromBody] BasketContractItem basketContractItem)
        {
            var addedItem = await _basketService.AddItemToBasketAsync(id, basketContractItem);
            if (addedItem != null) return addedItem;

            // ToDo Abstract this HttoResonseMessage generation
            var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request"),
                ReasonPhrase = $"Basket does not exist, or basketItem already exists. BasketId {id} / ContractItemId {basketContractItem.Id}"
            };
            throw new HttpResponseException(resp);
        }

        
        // Returns 204 on success / bad request on fail
        [HttpDelete]
        [Route("api/basket/{basketId}/item/{basketItemId}")]
        // Remove item from basket
        // DELETE: api/Basket/{basketId}/item/{basketItemId}
        public async Task<IHttpActionResult> Delete([FromUri] int basketId, [FromUri] int basketItemId)
        {

            var response = await _basketService.RemoveItemFromBasketAsync(basketId, basketItemId);

            if (response) return ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));

            var resp = GetResponseMessage(basketId, basketItemId);
            throw new HttpResponseException(resp);
        }

        
        [HttpDelete]
        [Route("api/basket/{basketId}/items")]
        // Remove all items from basket
        // DELETE: api/Basket/{basketId}/items
        // Returns BasketContract on success / bad request on fail
        public async Task<BasketContract> Delete(int basketId)
        {
            var removedFromBasket = await _basketService.RemoveBasketItemsAsync(basketId);
            if (removedFromBasket != null) return removedFromBasket;

            // ToDo Asumes BadRequest when Item cant be added
            var resp = GetResponseMessage(basketId);
            throw new HttpResponseException(resp);
        }


        // Helper Methods
        private HttpResponseMessage GetResponseMessage(int basketId, int? basketItemId = null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request"),
                ReasonPhrase = $"Basket or BasketItem does not exist. BasketId {basketId} / BasketItemId {basketItemId}"
            };
        }

    }
}
