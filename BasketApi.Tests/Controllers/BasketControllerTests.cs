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


        [TestMethod]
        public async Task BasketCreate()
        {
            var expectedBasketContract = GenerateBasketContract(1);
            var responseBasketContract = await _client.CreateBasket(expectedBasketContract);
            CompareExpectedAndActualBasketContract(expectedBasketContract, responseBasketContract);
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
