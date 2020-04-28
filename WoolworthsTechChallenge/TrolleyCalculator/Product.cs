namespace WoolworthsTechChallenge
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Discount
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

    public class Purchase
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
