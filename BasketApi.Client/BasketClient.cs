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
            var newBasketContract = await response.Content.ReadAsAsync<BasketContract>();
            return newBasketContract;
        }

    }
}
