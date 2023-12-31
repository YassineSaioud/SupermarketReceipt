﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;

namespace SupermarketReceipt
{
    public interface IDiscountCalculator
    {
        bool MtacheType(SpecialOfferType specialOfferType);
        Discount Calculate(Offer offer, Dictionary<Product, double> ProductQuantities, ISupermarketCatalog catalog);
    }


    public class BundleDiscountCalculator
        : IDiscountCalculator
    {

        public bool MtacheType(SpecialOfferType specialOfferType)
        {
            return specialOfferType == SpecialOfferType.Bundle;
        }

        public Discount Calculate(Offer offer, Dictionary<Product, double> productQuantities, ISupermarketCatalog catalog)
        {
            Discount discount = default;

            var bundleProducts = offer.Products;
            if (bundleProducts.All(productQuantities.ContainsKey))
            {
                double bundleProductsDiscountAmount = 0;

                foreach (var bundleProduct in bundleProducts)
                {
                    var minProductQuantity = (int)bundleProducts.Min(product => productQuantities.FirstOrDefault(productQuantity => productQuantity.Key == product).Value);
                    var unitPrice = catalog.GetUnitPrice(bundleProduct);
                    var discountAmount = -minProductQuantity * unitPrice * offer.Argument / 100.0;

                    bundleProductsDiscountAmount += discountAmount;
                }

                var description = $"{offer.Argument}% off bundle products";

                discount = new Discount(bundleProducts, description, bundleProductsDiscountAmount);
            }

            return discount;
        }
    }

    public class TenPercentDiscountCalculator
        : IDiscountCalculator
    {
        public bool MtacheType(SpecialOfferType specialOfferType)
        {
            return specialOfferType == SpecialOfferType.TenPercentDiscount;
        }

        public Discount Calculate(Offer offer, Dictionary<Product, double> productQuantities, ISupermarketCatalog catalog)
        {
            Discount discount = default;

            var product = offer.Product;
            if (productQuantities.ContainsKey(product))
            {
                var productQuantity = productQuantities[product];
                var unitPrice = catalog.GetUnitPrice(product);
                var description = $"{offer.Argument}% off";
                var discountAmount = -productQuantity * unitPrice * offer.Argument / 100.0;
                discount = new Discount(product, description, discountAmount);
            }

            return discount;
        }

    }


    public class TwoForAmountDiscountCalculator
        : IDiscountCalculator
    {

        public bool MtacheType(SpecialOfferType specialOfferType)
        {
            return specialOfferType == SpecialOfferType.TwoForAmount;
        }

        public Discount Calculate(Offer offer, Dictionary<Product, double> productQuantities, ISupermarketCatalog catalog)
        {
            Discount discount = default;

            var product = offer.Product;
            if (productQuantities.ContainsKey(product))
            {
                var productQuantity = productQuantities[product];
                var productQuantityAsInt = (int)productQuantity;
                if (productQuantityAsInt >= 2)
                {
                    var unitPrice = catalog.GetUnitPrice(product);
                    var offerArticles = 2;
                    var description = "2 for " + PrintPrice(offer.Argument);
                    var total = offer.Argument * (productQuantityAsInt / offerArticles) + productQuantityAsInt % 2 * unitPrice;
                    var discountAmount = unitPrice * productQuantity - total;
                    discount = new Discount(product, description, -discountAmount);
                }
            }

            return discount;
        }

        private static string PrintPrice(double price)
        {
            return price.ToString("N2", CultureInfo.CreateSpecificCulture("en-GB"));
        }

    }

    public class ThreeForTwoDiscountCalculator
        : IDiscountCalculator
    {

        public bool MtacheType(SpecialOfferType specialOfferType)
        {
            return specialOfferType == SpecialOfferType.ThreeForTwo;
        }

        public Discount Calculate(Offer offer, Dictionary<Product, double> productQuantities, ISupermarketCatalog catalog)
        {
            Discount discount = default;

            var product = offer.Product;
            if (productQuantities.ContainsKey(product))
            {
                var productQuantity = productQuantities[product];
                var productQuantityAsInt = (int)productQuantity;
                if (productQuantityAsInt > 2)
                {
                    var unitPrice = catalog.GetUnitPrice(product);
                    var offerArticles = 3;
                    var articlesToConsider = productQuantityAsInt / offerArticles;
                    var description = "3 for 2";
                    var discountAmount = productQuantity * unitPrice - (articlesToConsider * 2 * unitPrice + productQuantityAsInt % 3 * unitPrice);
                    discount = new Discount(product, description, -discountAmount);
                }
            }

            return discount;
        }

    }


    public class FiveForAmountDiscountCalculator
       : IDiscountCalculator
    {

        public bool MtacheType(SpecialOfferType specialOfferType)
        {
            return specialOfferType == SpecialOfferType.FiveForAmount;
        }

        public Discount Calculate(Offer offer, Dictionary<Product, double> productQuantities, ISupermarketCatalog catalog)
        {
            Discount discount = default;

            var product = offer.Product;
            if (productQuantities.ContainsKey(product))
            {
                var productQuantity = productQuantities[product];
                var productQuantityAsInt = (int)productQuantity;
                if (productQuantityAsInt >= 5)
                {
                    var unitPrice = catalog.GetUnitPrice(product);
                    var offerArticles = 5;
                    var articlesToConsider = productQuantityAsInt / offerArticles;
                    var description = $"{offerArticles} for {PrintPrice(offer.Argument)}";
                    var discountTotal = unitPrice * productQuantity - (offer.Argument * articlesToConsider + productQuantityAsInt % 5 * unitPrice);
                    discount = new Discount(product, description, -discountTotal);
                }
            }

            return discount;
        }

        private static string PrintPrice(double price)
        {
            return price.ToString("N2", CultureInfo.CreateSpecificCulture("en-GB"));
        }

    }


}
