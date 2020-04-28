using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge.Tests
{
    public class SortServiceTests
    {
        private List<Product> data;

        [SetUp]
        public void Setup()
        {
            var dataJson = @"[{
                            'name': 'Test Product A',
                            'price': 99.99,
                            'quantity': 1
                          },
                          {
                            'name': 'Test Product B',
                            'price': 101.99,
                            'quantity': 1
                          },
                          {
                            'name': 'Test Product C',
                            'price': 10.99,
                            'quantity': 1
                          }]";

            data = JsonConvert.DeserializeObject<List<Product>>(dataJson);
        }

        [Test]
        [TestCase("Low", ExpectedResult = "Test Product C")]
        [TestCase("High", ExpectedResult = "Test Product B")]
        [TestCase("Ascending", ExpectedResult = "Test Product A")]
        [TestCase("Descending", ExpectedResult = "Test Product C")]
        public async Task<string> SortService_ShouldSortByPrice_WhenSortOptionIsLow(string sortOption)
        {
            var shopperHistoryClient = Substitute.For<IShopperHistoryClient>();

            var sut = new SortService(shopperHistoryClient);
            var sortedProducts = await sut.Sort(sortOption, data);

            return sortedProducts.First().Name;
        }

        [Test]
        public async Task SortService_ShouldSortByPurchasedQuantities_WhenSortOptionIsRecommended()
        {
            var shopperHistoryJson = @"[{
                                            'customerId': 23,
                                            'products': [
                                                {
                                                'name': 'Test Product A',
                                                'price': 99.99,
                                                'quantity': 2
                                                },
                                                {
                                                'name': 'Test Product B',
                                                'price': 101.99,
                                                'quantity': 3
                                                },
                                                {
                                                'name': 'Test Product F',
                                                'price': 999999999999,
                                                'quantity': 1
                                                }
                                            ]
                                        }]";
            var shopperHistoryClient = Substitute.For<IShopperHistoryClient>();
            var shopperHistoryData = JsonConvert.DeserializeObject<IEnumerable<ShopperHistory>>(shopperHistoryJson);
            shopperHistoryClient.GetShopperHistory().Returns(shopperHistoryData);

            var sut = new SortService(shopperHistoryClient);
            var sortedProducts = await sut.Sort("Recommended", data);

            Assert.AreEqual("Test Product B", sortedProducts.First().Name);
        }

        [Test]
        public async Task SortService_ShouldCallShopperHistoryApi_WhenSortOptionIsRecommended()
        {
            var shopperHistoryClient = Substitute.For<IShopperHistoryClient>();

            var sut = new SortService(shopperHistoryClient);
            var sortedProducts = await sut.Sort("Recommended", data);

            await shopperHistoryClient.Received(1).GetShopperHistory();
        }
    }
}