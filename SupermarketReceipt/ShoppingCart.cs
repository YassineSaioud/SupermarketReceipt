using System.Collections.Generic;
using System.Linq;

namespace SupermarketReceipt
{
    public class ShoppingCart
    {
        private readonly List<ProductQuantity> _items = new();
        private readonly Dictionary<Product, double> _productQuantities = new();

        private readonly List<IDiscountCalculator> _discountCalculators;

        public ShoppingCart(List<IDiscountCalculator> discountCalculators)
        {
            _discountCalculators = discountCalculators;
        }

        public List<ProductQuantity> GetItems()
        {
            return new List<ProductQuantity>(_items);
        }

        public void AddItem(Product product)
        {
            AddItemQuantity(product, 1.0);
        }

        public void AddItemQuantity(Product product, double quantity)
        {
            _items.Add(new ProductQuantity(product, quantity));

            if (_productQuantities.ContainsKey(product))
            {
                var newAmount = _productQuantities[product] + quantity;
                _productQuantities[product] = newAmount;
            }
            else
            {
                _productQuantities.Add(product, quantity);
            }
        }

        public void HandleOffers(Receipt receipt, List<Offer> offers, ISupermarketCatalog catalog)
        {
            if (offers.Any())
            {
                foreach (var offer in offers)
                {
                    var discount = _discountCalculators.FirstOrDefault(c => c.MtacheType(offer.OfferType))
                                                      ?.Calculate(offer, _productQuantities, catalog);
                    if (discount != null)
                    {
                        receipt.AddDiscount(discount);
                    }
                }
            }
        }

    }
}