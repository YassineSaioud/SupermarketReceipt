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
            catalog.AddProduct(toothbrush, 7);

            var apples = new Product("apples", ProductUnit.Kilo);
            catalog.AddProduct(apples, 2);

            var discountCalculators = new List<IDiscountCalculator> { new TenPercentDiscountCalculator() };
            var shoppingCart = new ShoppingCart(discountCalculators);
            shoppingCart.AddItemQuantity(toothbrush, 5);
            shoppingCart.AddItemQuantity(apples, 2.5);

            var teller = new Teller(catalog);
            var offerArgument = 10.0;
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, toothbrush, offerArgument);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

            // ASSERT
            var expectedToothbrushReceiptItem = receipt.GetItems()[0];
            var expectedApplesReceiptItem = receipt.GetItems()[1];
            var expectedToothbrushDiscount = expectedToothbrushReceiptItem.TotalPrice * offerArgument / 100.0;
            var expectedTotalPriceOfAllProducts = (expectedToothbrushReceiptItem.TotalPrice - expectedToothbrushDiscount) + expectedApplesReceiptItem.TotalPrice;

            Assert.Equal(expectedToothbrushReceiptItem.Product, toothbrush);
            Assert.Equal(expectedApplesReceiptItem.Product, apples);
            Assert.Equal(-expectedToothbrushDiscount, receipt.GetDiscounts().FirstOrDefault().DiscountAmount);
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
            var offerArgument = 10.0;
            teller.AddSpecialOffer(SpecialOfferType.Bundle, bundleProducts, offerArgument);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

            // ASSERT
            var expectedBundleReceiptItems = receipt.GetItems();
            var expectedProducts = expectedBundleReceiptItems.Select(s => s.Product);
            var bundleProductsTotalPrice = expectedBundleReceiptItems.Sum(s => s.TotalPrice);
            var expectedBundleProductsDiscount = bundleProductsTotalPrice * offerArgument / 100.0;
            var expectedbundleProductsTotalPrice = bundleProductsTotalPrice - expectedBundleProductsDiscount;

            Assert.Equal(expectedProducts.ToList()[0], toothbrush);
            Assert.Equal(expectedProducts.ToList()[1], toothpaste);
            Assert.Equal(-expectedBundleProductsDiscount, receipt.GetDiscounts().FirstOrDefault().DiscountAmount);
            Assert.Equal(expectedbundleProductsTotalPrice, receipt.GetTotalPrice());

        }

        [Fact]
        public void OnlyCompleteBundleDiscount()
        {
            // ARRANGE
            ISupermarketCatalog catalog = new FakeCatalog();

            var toothbrush = new Product("toothbrush", ProductUnit.Each);
            catalog.AddProduct(toothbrush, 7);

            var toothpaste = new Product("toothpaste", ProductUnit.Each);
            catalog.AddProduct(toothpaste, 4);

            var discountCalculators = new List<IDiscountCalculator> { new BundleDiscountCalculator() };
            var shoppingCart = new ShoppingCart(discountCalculators);
            shoppingCart.AddItemQuantity(toothbrush, 2);
            shoppingCart.AddItemQuantity(toothpaste, 1);

            var teller = new Teller(catalog);
            var bundleProducts = new List<Product> { toothbrush, toothpaste };
            var offerArgument = 10.0;
            teller.AddSpecialOffer(SpecialOfferType.Bundle, bundleProducts, offerArgument);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(shoppingCart);

            // ASSERT
            var expectedBundleReceiptItems = receipt.GetItems();
            var expectedProducts = expectedBundleReceiptItems.Select(s => s.Product);
            var bundleProductMinQuantity = (int)expectedBundleReceiptItems.Min(s => s.Quantity);
            var expectedOnlyCompleteProductsDiscount = bundleProductMinQuantity * expectedBundleReceiptItems.Sum(s => s.Price) * offerArgument / 100.0;
            var expectedProductsTotalPrice = expectedBundleReceiptItems.Sum(s => s.TotalPrice) - expectedOnlyCompleteProductsDiscount;

            Assert.Equal(expectedProducts.ToList()[0], toothbrush);
            Assert.Equal(expectedProducts.ToList()[1], toothpaste);
            Assert.Equal(-expectedOnlyCompleteProductsDiscount, receipt.GetDiscounts().FirstOrDefault().DiscountAmount);
            Assert.Equal(expectedProductsTotalPrice, receipt.GetTotalPrice());

        }

    }
}