using System.Collections.Generic;

namespace WoolworthsTechChallenge.Controllers
{
    public class TrolleyRequest
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Special> Specials { get; set; }
        public IEnumerable<NameQuantity> Quantities { get; set; }
    }
}
