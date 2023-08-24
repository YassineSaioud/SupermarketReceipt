namespace SupermarketReceipt
{
    public class Product
    {
        public Product(string name, ProductUnit unit)
        {
            Name = name;
            Unit = unit;
        }

        public string Name { get; }
        public ProductUnit Unit { get; }
    }

    public class ProductQuantity
    {
        public ProductQuantity(Product product, double weight)
        {
            Product = product;
            Quantity = weight;
        }

        public Product Product { get; }
        public double Quantity { get; }
    }

    public enum ProductUnit
    {
        Kilo,
        Each
    }
}