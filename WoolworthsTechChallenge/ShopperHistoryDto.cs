using System.Collections.Generic;

namespace WoolworthsTechChallenge
{
    public class ShopperHistoryDto
    {
        public int CustomerId { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
