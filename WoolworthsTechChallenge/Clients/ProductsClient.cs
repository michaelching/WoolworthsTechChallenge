using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WoolworthsTechChallenge
{

    public class ProductsClient : IProductsClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductsClient> _logger;

        public ProductsClient(IHttpClientFactory httpClientFactory, ILogger<ProductsClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["ResourceApiBaseUrl"]);
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var productsUrl = new Uri($"/api/resource/products?token={Constants.UserToken}", UriKind.Relative);
            var products = await GetProducts(productsUrl);

            return products.Select(p => ApiResourceMapper.ToProduct(p));
        }

        private async Task<IEnumerable<ProductDto>> GetProducts(Uri productUri)
        {
            try
            {
                var response = await _httpClient.GetAsync(productUri);
                response.EnsureSuccessStatusCode();
                var stringContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(stringContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error connecting to Products API: {ex.Message}");
                throw;
            }
        }
    }
}
