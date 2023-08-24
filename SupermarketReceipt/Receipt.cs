using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Receipt
    {
        private readonly List<ReceiptItem> _items = new();
        private readonly List<Discount> _discounts = new();
       
     
        public void AddProduct(Product p, double quantity, double price, double totalPrice)
        {
            _items.Add(new ReceiptItem(p, quantity, price, totalPrice));
        }

        public void AddDiscount(Discount discount)
        {
            _discounts.Add(discount);
        }

        public List<ReceiptItem> GetItems()
        {
            return new List<ReceiptItem>(_items);
        }   

        public List<Discount> GetDiscounts()
        {
            return _discounts;
        }

        public double GetTotalPrice()
        {
            var total = 0.0;

            foreach (var item in _items)
            {
                total += item.TotalPrice;
            }

            foreach (var discount in _discounts)
            {
                total += discount.DiscountAmount;
            }

            return total;
        }
    }

    public class ReceiptItem
    {
        public ReceiptItem(Product product, double quantity, double price, double totalPrice)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
            TotalPrice = totalPrice;
        }

        public Product Product { get; }
        public double Price { get; }
        public double TotalPrice { get; }
        public double Quantity { get; }
    }

}