﻿using System.Collections.Generic;

namespace WoolworthsTechChallenge
{
    public class ShopperHistory
    {
        public int CustomerId { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
