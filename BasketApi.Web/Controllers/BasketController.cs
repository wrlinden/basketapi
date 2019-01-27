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
    }
}
