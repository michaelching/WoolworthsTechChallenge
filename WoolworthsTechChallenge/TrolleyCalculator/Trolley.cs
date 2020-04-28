using System.Collections.Generic;

namespace WoolworthsTechChallenge
{
    public class Trolley
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Discount> Discounts { get; set; }
        public IEnumerable<Purchase> Purchases { get; set; }
    }
}
