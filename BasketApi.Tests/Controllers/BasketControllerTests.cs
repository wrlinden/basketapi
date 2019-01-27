using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using BasketApi.Client;
using BasketApi.Contracts;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BasketApi.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests : IDisposable
    {

        private static IDisposable _owinServer;
        private static BasketClient _client;
        private static readonly Fixture Fixture = new Fixture();
        private const string BaseAddress = "http://localhost:9001/";


        [TestInitialize]
        public void Setup()
        {
            _owinServer = WebApp.Start<Startup>(url: BaseAddress);
            _client = new BasketClient(BaseAddress);
        }
        [TestCleanup]
        public void TearDown()
        {
            this.Dispose();
        }
        public void Dispose()
        {
            _owinServer.Dispose();
        }


        [TestMethod]
        public async Task BasketCreate()
        {
            var expectedBasketContract = GenerateBasketContract(1);
            var responseBasketContract = await _client.CreateBasket(expectedBasketContract);
            CompareExpectedAndActualBasketContract(expectedBasketContract, responseBasketContract);
        }


        [TestMethod]
        public async Task GetBasketById()
        {

            // Create a Basket with one Item in it
            var expectedBasketContract = GenerateBasketContract(1);
            var createdResponseBasketContract = await _client.CreateBasket(expectedBasketContract);

            // Test the returned response has the correct values
            Assert.AreEqual(expectedBasketContract.Id, createdResponseBasketContract.Id);
            Assert.AreEqual(expectedBasketContract.Items[0].ContractItem.Description, createdResponseBasketContract.Items[0].ContractItem.Description);

            // Test the data comes back correct from the API
            var response = await _client.GetBasketById(expectedBasketContract.Id);
            CompareExpectedAndActualBasketContract(expectedBasketContract, response);

        }


        [TestMethod]
        public async Task AddItemToBasket()
        {
            var expectedBasketContract = GenerateBasketContract(1);
            await _client.CreateBasket(expectedBasketContract);
            var expectedBasketContractItem = Fixture.Create<BasketContractItem>();

            var responseBasketContractItem = await _client.AddBasketItem(expectedBasketContract.Id, expectedBasketContractItem);
            CompareExpectedAndActualContractItem(expectedBasketContractItem, responseBasketContractItem);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task CreatingDupplicateItemThrowsException()
        {
            // Setup a new basket and try to re-add existing basketItem to it 
            var basketContract = GenerateBasketContract(1);
            var createdBasketContract = await _client.CreateBasket(basketContract);
            await _client.AddBasketItem(1, createdBasketContract.Items[0]);

        }

        [TestMethod]
        public async Task RemoveItemFromBasket()
        {

            var expectedBasketContract = GenerateBasketContract(1);
            var responseBasketContract = await _client.CreateBasket(expectedBasketContract);

            var basketContractItemToRemove = responseBasketContract.Items[0];

            // Remove an existing basket item
            var responseRemovedFromBasket = await _client.RemoveFromBasket(responseBasketContract.Id, basketContractItemToRemove.Id);
            Assert.AreEqual(true, responseRemovedFromBasket);

            // Confirm item has been removed 
            var basketWithRemovedItem = await _client.GetBasketById(expectedBasketContract.Id);
            var containsItem = basketWithRemovedItem.Items.Contains(basketContractItemToRemove);
            Assert.AreEqual(false, containsItem);

        }



        // Helper Methods
        private static IList<BasketContractItem> GenerateBasketContractItems(int number)
        {
            var generatedContractBasketItems = Fixture.CreateMany<BasketContractItem>(number).ToList();
            foreach (var generatedContractBasketItem in generatedContractBasketItems)
            {
                generatedContractBasketItem.ContractItem = Fixture.Create<ContractItem>();
            }
            return generatedContractBasketItems;
        }

        private static void CompareExpectedAndActualBasketContract(BasketContract expectedBasketContract, BasketContract actualBasketContract)
        {
            Assert.AreEqual(expectedBasketContract.Id, actualBasketContract.Id);
            for (var index = expectedBasketContract.Items.Count - 1; index >= 0; index--)
            {
                CompareExpectedAndActualContractItem(expectedBasketContract.Items[index], actualBasketContract.Items[index]);
            }

            Assert.AreEqual(expectedBasketContract.Items[0].Qty, actualBasketContract.Items[0].Qty);
        }

        private static void CompareExpectedAndActualContractItem(BasketContractItem expectedBasketContractItem, BasketContractItem actualBasketContractItem)
        {
            Assert.AreEqual(expectedBasketContractItem.Id, actualBasketContractItem.Id);
            Assert.AreEqual(expectedBasketContractItem.Qty, actualBasketContractItem.Qty);
            Assert.AreEqual(expectedBasketContractItem.Id, actualBasketContractItem.Id);
            Assert.AreEqual(expectedBasketContractItem.ContractItem.Description, actualBasketContractItem.ContractItem.Description);
            Assert.AreEqual(expectedBasketContractItem.ContractItem.Price, actualBasketContractItem.ContractItem.Price);
        }

        private static BasketContract GenerateBasketContract(int number)
        {
            return new BasketContract
            {
                Id = Fixture.Create<int>(),
                Items = GenerateBasketContractItems(number)
            };
        }

    }
}
