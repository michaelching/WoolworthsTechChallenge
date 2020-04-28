using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge
{
    public class SortService : ISortService
    {
        private readonly IShopperHistoryClient _shopperHistoryClient;

        public SortService(IShopperHistoryClient shopperHistoryClient)
        {
            _shopperHistoryClient = shopperHistoryClient;
        }

        public async Task<IEnumerable<Product>> Sort(string sortOption, IEnumerable<Product> products)
        {
            switch (sortOption)
            {
                case "Low":
                    return products.OrderBy(p => p.Price);
                case "High":
                    return products.OrderByDescending(p => p.Price);
                case "Ascending":
                    return products.OrderBy(p => p.Name);
                case "Descending":
                    return products.OrderByDescending(p => p.Name);
                case "Recommended":
                    return await OrderByRecommendation(products);
                default:
                    return products;
            }
        }

        private async Task<IEnumerable<Product>> OrderByRecommendation(IEnumerable<Product> products)
        {
            var shopperHistory = await _shopperHistoryClient.GetShopperHistory();

            var shopperPopularity = shopperHistory
                .SelectMany(s => s.Products)
                .GroupBy(p => p.Name, (key, result) => (name: key, popularity: result.Sum(r => r.Quantity)))
                .ToDictionary(g => g.name, g => g.popularity);

            var productsPopularity = products.Select(p =>
            {
                var popularity = shopperPopularity.ContainsKey(p.Name) ? shopperPopularity[p.Name] : 0;
                return (popularity, product: p);
            });

            return productsPopularity.OrderByDescending(p => p.popularity).Select(p => p.product);
        }
    }

    
}
