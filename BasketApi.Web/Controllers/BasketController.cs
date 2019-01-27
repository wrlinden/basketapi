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
    }
}
