using System.Collections.Generic;

namespace WoolworthsTechChallenge.Controllers
{
    public class Special
    {
        public decimal Total { get; set; }
        public IEnumerable<NameQuantity> Quantities { get; set; }
    }
}
