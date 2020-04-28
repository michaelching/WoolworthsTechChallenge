using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WoolworthsTechChallenge
{
    public class TrolleyCalculatorClient : ITrolleyCalculatorClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TrolleyCalculatorClient> _logger;

        public TrolleyCalculatorClient(IHttpClientFactory httpClientFactory, ILogger<TrolleyCalculatorClient> logger, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(configuration["ResourceApiBaseUrl"]);
            _logger = logger;
        }

        public async Task<decimal> CalculateTotal(string trolleyJson)
        {
            var trolleyCalculatorUrl = new Uri($"/api/resource/trolleyCalculator?token={Constants.UserToken}", UriKind.Relative);

            var total = await CalculateTrolleyTotal(trolleyCalculatorUrl, trolleyJson);

            return total;
        }

        private async Task<decimal> CalculateTrolleyTotal(Uri trolleyTotalUri, string trolleyJson)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await _httpClient.PostAsync(trolleyTotalUri, new StringContent(trolleyJson, Encoding.UTF8, "application/json-patch+json"));

                response.EnsureSuccessStatusCode();

                var stringContent = await response.Content.ReadAsStringAsync();

                return Convert.ToDecimal(stringContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"Error connecting to TrolleyTotal API: {ex.Message}");
                throw;
            }
        }
    }
}
