using System.Collections.Generic;

namespace SupermarketReceipt
{
    public class Teller
    {
        private readonly ISupermarketCatalog _catalog;
        private readonly List<Offer> _offers = new();

        public Teller(ISupermarketCatalog catalog)
        {
            _catalog = catalog;
        }

        public void AddSpecialOffer(SpecialOfferType offerType, Product product, double argument)
        {
            _offers.Add(new Offer(offerType, product, argument));
        }

        public void AddSpecialOffer(SpecialOfferType offerType, List<Product> products, double argument)
        {
            _offers.Add(new Offer(offerType, products, argument));
        }

        public Receipt ChecksOutArticlesFrom(ShoppingCart shoppingCart)
        {
            var receipt = new Receipt();
            var productQuantities = shoppingCart.GetItems();

            foreach (var productQuantity in productQuantities)
            {
                var product = productQuantity.Product;
                var quantity = productQuantity.Quantity;
                var unitPrice = _catalog.GetUnitPrice(product);
                var price = quantity * unitPrice;

                receipt.AddProduct(product, quantity, unitPrice, price);
            }

            shoppingCart.HandleOffers(receipt, _offers, _catalog);

            return receipt;
        }
    }
}