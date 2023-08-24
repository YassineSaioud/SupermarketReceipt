using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace SupermarketReceipt.Test
{
    public class SupermarketXUnitTest
    {
        [Fact]
        public void TenPercentDiscount()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();

            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 2);

            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 2);

            var discountCalculators = new List<IDiscountCalculator> { new TenPercentDiscountCalculator() };
            var shoppingCart = new ShoppingCart(discountCalculators);
            shoppingCart.AddItemQuantity(toothbrush, 5);
            shoppingCart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog);
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

            // ASSERT
            var expectedToothbrushReceiptItem = receipt.GetItems()[0];
            var expectedToothbrushDiscount = expectedToothbrushReceiptItem.TotalPrice * 0.1;
            var expectedTotalPriceOfAllProducts = expectedToothbrushReceiptItem.TotalPrice - expectedToothbrushDiscount + receipt.GetItems()[1].TotalPrice;

            Assert.Equal(expectedToothbrushReceiptItem.Product, toothbrush);
            Assert.Equal(expectedToothbrushDiscount, Math.Abs(receipt.GetDiscounts().FirstOrDefault().DiscountAmount));
            Assert.Equal(expectedTotalPriceOfAllProducts, receipt.GetTotalPrice());

        }

        [Fact]
        public void BundleDiscount()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();

            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 7);

            var toothpaste = new Product("toothpaste", ProductUnit.Each);
            catalog.AddProduct(toothpaste, 4);

            var discountCalculators = new List<IDiscountCalculator> { new BundleDiscountCalculator() };
            var shoppingCart = new ShoppingCart(discountCalculators);
            shoppingCart.AddItemQuantity(toothbrush, 1);
            shoppingCart.AddItemQuantity(toothpaste, 1);

            var teller = new Teller(catalog);
            var bundleProducts = new List<Product> { toothbrush, toothpaste };
            teller.AddSpecialOffer(SpecialOfferType.Bundle, bundleProducts, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

            // ASSERT
            var expectedBundleReceiptItems = receipt.GetItems();
            var expectedProducts = expectedBundleReceiptItems.Select(s => s.Product);
            var bundleProductsTotalPrice = expectedBundleReceiptItems.Sum(s => s.TotalPrice);
            var expectedBundleProductsDiscount = bundleProductsTotalPrice * 0.1;
            var expectedbundleProductsTotalPrice = bundleProductsTotalPrice - expectedBundleProductsDiscount;

            Assert.Equal(expectedProducts.ToList()[0], toothbrush);
            Assert.Equal(expectedProducts.ToList()[1], toothpaste);
            Assert.Equal(expectedBundleProductsDiscount, Math.Abs(receipt.GetDiscounts().FirstOrDefault().DiscountAmount));
            Assert.Equal(expectedbundleProductsTotalPrice, receipt.GetTotalPrice());

        }

    }
}