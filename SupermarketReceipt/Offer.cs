using System.Collections.Generic;

namespace SupermarketReceipt
{
    public enum SpecialOfferType
    {
        ThreeForTwo,
        TenPercentDiscount,
        TwoForAmount,
        FiveForAmount,
        Bundle
    }

    public class Offer
    {
        public Offer(SpecialOfferType offerType, Product product, double argument)
        {
            OfferType = offerType;
            Argument = argument;
            Product = product;
        }

        public Offer(SpecialOfferType offerType, List<Product> products, double argument)
        {
            OfferType = offerType;
            Products = products;
            Argument = argument;
        }

        public SpecialOfferType OfferType { get; }
        public Product Product { get; }
        public List<Product> Products { get; }
        public double Argument { get; }
    }

}