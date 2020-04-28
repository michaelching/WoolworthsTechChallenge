using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge.Controllers
{
    [ApiController]
    [Route("api/answers")]
    public class AnswersController : ControllerBase
    {
        private readonly IProductsClient _productsClient;
        private readonly ISortService _sortService;
        private readonly ITrolleyCalculatorClient _trolleyCalculatorClient;

        public AnswersController(IProductsClient productsClient, ISortService sortService, ITrolleyCalculatorClient trolleyCalculatorClient)
        {
            _productsClient = productsClient;
            _sortService = sortService;
            _trolleyCalculatorClient = trolleyCalculatorClient;
        }

        [HttpGet("user")]
        public IActionResult Exercise1()
        {
            var response = new
            {
                name = Constants.Name,
                token = Constants.UserToken.ToString()
            };
            return Content(JsonConvert.SerializeObject(response));
        }

        [HttpGet("sort")]
        public async Task<IActionResult> Exercise2([FromQuery] string sortOption)
        {
            var products = await _productsClient.GetProducts();
            var sortedProducts = await _sortService.Sort(sortOption, products);

            return Content(JsonConvert.SerializeObject(sortedProducts));
        }

        [HttpPost("trolleyTotal")]
        public async Task<IActionResult> Exercise3([FromBody] TrolleyRequest request)
        {
            var trolleyJson = JsonConvert.SerializeObject(request);
            var totalPrice = await _trolleyCalculatorClient.CalculateTotal(trolleyJson);

            return Content(totalPrice.ToString());
        }
    }
}
