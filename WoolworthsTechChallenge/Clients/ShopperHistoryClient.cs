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
    public class ShopperHistoryClient : IShopperHistoryClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ShopperHistoryClient> _logger;

        public ShopperHistoryClient(IHttpClientFactory httpClientFactory, ILogger<ShopperHistoryClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["ResourceApiBaseUrl"]);
            _logger = logger;
        }

        public async Task<IEnumerable<ShopperHistory>> GetShopperHistory()
        {
            var shopperHistoryUrl = new Uri($"/api/resource/shopperHistory?token={Constants.UserToken}", UriKind.Relative);
            
            var shopperHistories = await GetShopperHistory(shopperHistoryUrl);

            return shopperHistories.Select(s => ApiResourceMapper.ToShopperHistory(s));
        }

        private async Task<IEnumerable<ShopperHistoryDto>> GetShopperHistory(Uri shopperHistoryUri)
        {
            try
            {
                var response = await _httpClient.GetAsync(shopperHistoryUri);
                response.EnsureSuccessStatusCode();
                var stringContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ShopperHistoryDto>>(stringContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error connecting to ShopperHistory API: {ex.Message}");
                throw;
            }
        }
    }
}
