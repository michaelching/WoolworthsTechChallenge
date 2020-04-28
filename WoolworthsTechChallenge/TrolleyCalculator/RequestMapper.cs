using System.Linq;
using WoolworthsTechChallenge.Controllers;

namespace WoolworthsTechChallenge
{
    public static class RequestMapper
    {
        private static Discount ToDiscount(Special special)
        {
            var firstSpecial = special.Quantities.First();

            return new Discount
            {
                Name = firstSpecial.Name,
                Quantity = firstSpecial.Quantity,
                Price = special.Total
            };
        }

        private static Purchase ToPurchase(NameQuantity nameQuantity)
        {
            return new Purchase
            {
                Name = nameQuantity.Name,
                Quantity = nameQuantity.Quantity
            };
        }

        public static Trolley ToTrolley(TrolleyRequest request)
        {
            return new Trolley
            {
                Products = request.Products,
                Discounts = request.Specials.Select(s => ToDiscount(s)),
                Purchases = request.Quantities.Select(q => ToPurchase(q))
            };
        }
    
    }

}
