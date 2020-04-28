using System;
using System.Linq;

namespace WoolworthsTechChallenge
{
    public static class ApiResourceMapper
    {
        public static Product ToProduct(ProductDto p)
        {
            return new Product
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = Convert.ToInt32(p.Quantity)
            };
        }

        public static ShopperHistory ToShopperHistory(ShopperHistoryDto s)
        {
            return new ShopperHistory
            {
                CustomerId = s.CustomerId,
                Products = s.Products.Select(p => ToProduct(p))
            };
        }
    }

}
