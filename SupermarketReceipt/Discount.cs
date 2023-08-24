using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Discount
    {
        public Discount(Product product, string description, double discountAmount)
        {
            Product = product;
            Description = description;
            DiscountAmount = discountAmount;
        }

        public Discount(List<Product> products, string description, double discountAmount)
        {
            Description = description;
            Products = products;
            DiscountAmount = discountAmount;
        }

        public string Description { get; }
        public double DiscountAmount { get; }
        public Product Product { get; }
        public List<Product> Products { get; set; }

    }
}