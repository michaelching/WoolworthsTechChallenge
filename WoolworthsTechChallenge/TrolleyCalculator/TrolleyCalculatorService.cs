using System;
using System.Collections.Generic;
using System.Linq;

namespace WoolworthsTechChallenge
{
    public interface ITrolleyCalculatorService
    {
        decimal CalculateTotal(Trolley trolley);
    }

    public class TrolleyCalculatorService : ITrolleyCalculatorService
    {
        private class QuantityPrice
        {
            public int Quantity { get; set; }
            public decimal Price { get; set; }

            public QuantityPrice(int quantity, decimal price)
            {
                Quantity = quantity;
                Price = price;
            }
        }

        public decimal CalculateTotal(Trolley trolley)
        {
            var purchaseCosts = trolley.Purchases.Select(purchase => 
            {
                var pricingList = GetPricingList(trolley, purchase.Name);

                // Initial State
                var possibleTotals = new List<decimal>();
                var lowestPriceQuantitiesSoFar = new Dictionary<int, decimal>();

                var subtotals = new List<QuantityPrice>();
                subtotals.Add(new QuantityPrice(purchase.Quantity, 0));
                var newSubtotals = new List<QuantityPrice>();

                while (subtotals.Count > 0)
                {
                    // Map
                    foreach (var priceQuantity in pricingList)
                    {
                        var nextSubtotals = subtotals.Select(subtotal => new QuantityPrice(subtotal.Quantity - priceQuantity.Key, subtotal.Price + priceQuantity.Value));
                        newSubtotals.AddRange(nextSubtotals);
                    };

                    // Reduce
                    possibleTotals.AddRange(newSubtotals.Where(ns => ns.Quantity == 0).Select(ns => ns.Price));
                    newSubtotals.RemoveAll(ns => ns.Quantity <= 0);
                    newSubtotals = newSubtotals.GroupBy(ns => ns.Quantity, (key, result) => new QuantityPrice(key, result.Select(r => r.Price).Min())).ToList();

                    // Increment
                    subtotals = newSubtotals;
                    newSubtotals = new List<QuantityPrice>();
                }

                return possibleTotals.Min();
            });

            return purchaseCosts.Sum();
        }

        public Dictionary<int, decimal> GetPricingList(Trolley trolley, string productName)
        {
            var discounts = trolley.Discounts.Where(s => s.Name == productName);
            var product = trolley.Products.Single(p => p.Name == productName);

            var pricingList = new Dictionary<int, decimal>();

            pricingList.Add(1, product.Price);

            foreach(var discount in discounts)
            {
                if (pricingList.ContainsKey(discount.Quantity))
                {
                    pricingList[discount.Quantity] = Math.Min(discount.Price, pricingList[discount.Quantity]);
                }
                else
                {
                    pricingList.Add(discount.Quantity, discount.Price);
                }
            }

            return pricingList;
        }
    }

    
}
