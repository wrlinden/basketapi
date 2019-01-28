using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BasketApi.Contracts;
using BasketApi.Services;
using Newtonsoft.Json;

namespace BasketApi.Controllers
{
    public class BasketController : ApiController
    {

        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        // Create a new Basket
        // POST: api/Basket
        public async Task<BasketContract> Post([FromBody] BasketContract basket)
        {
            var result = await _basketService.CreateBasketAsync(basket);
            if (result == null) ThrowBadRequestException($"The basket could not be created. Request : ${JsonConvert.SerializeObject(basket)}");
            return result;
        }

        
        [HttpGet]
        // Get basket by Id
        // GET: api/Basket/{id}
        public async Task<BasketContract> Get([FromUri] int id)
        {
            var basket = await _basketService.GetBasket(id);
            if (basket == null) ThrowBadRequestException($"The basket could not be found. BasketId : ${id}");
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
            var resp = GetResponseMessage(id);
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

            // ToDo : Add test to remove an item that does not exist
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

            // ToDo Asumes BadRequest when Item cant be deleted
            var resp = GetResponseMessage(basketId);
            throw new HttpResponseException(resp);
        }


        
        [HttpPut]
        [Route("api/basket/{basketId}/item/{basketItemId}")]
        // PUT: api/Basket/{id}/item/{id}
        // Use for incrementing or decrementing basket item quantities (client contains helper methods to seperate add and remove functionality)
        // Returns the updated BasketContractItem
        public async Task<BasketContractItem> Put([FromUri] int basketId, [FromUri] int basketItemId, [FromBody] int qty)
        {
            // Treats qty = zero the same as any, for performance this should/could be changed.

            var addedItem = await _basketService.AddToBasketItemAsync(basketId, basketItemId, qty);
            if (addedItem != null) return addedItem;

            // ToDo Asumes BadRequest when Item cant be added
            var resp = GetResponseMessage(basketId, basketItemId);
            throw new HttpResponseException(resp);
        }

        // Helper Methods
        private static HttpResponseMessage GetResponseMessage(int basketId, int? basketItemId = null)
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Bad request"),
                ReasonPhrase = $"Basket or BasketItem does not exist. BasketId {basketId} / BasketItemId {basketItemId}"
            };
        }

        private static HttpResponseMessage ThrowBadRequestException(string message)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent(message),
                ReasonPhrase = message
            };

            throw new HttpResponseException(responseMessage);
        }

    }
}
