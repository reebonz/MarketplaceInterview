using System.Collections.Generic;
using NUnit.Framework;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class ShippingOptionTests
    {
        [Test]
        public void FlatRateShippingOptionTest()
        {
            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.5m};
            var shippingAmount = flatRateShippingOption.GetAmount(new LineItem(), new Basket());

            Assert.That(shippingAmount, Is.EqualTo(1.5m), "Flat rate shipping not correct.");
        }

        [Test]
        public void PerRegionShippingOptionTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
                                              {
                                                  PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
                                              };

            var shippingAmount = perRegionShippingOption.GetAmount(new LineItem() {DeliveryRegion = RegionShippingCost.Regions.Europe}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void PerRegionDiscountShippingOptionTest() {
            var perRegionDiscountShippingOption = new PerRegionDiscountShipping()
            {
                SpecialRule_deducte = .5m,
                PerRegionCosts = new[]
                {
                    new RegionShippingCost()
                    {
                        DestinationRegion = RegionShippingCost.Regions.UK,
                        Amount = 6m
                    },
                    new RegionShippingCost()
                    {
                        DestinationRegion = RegionShippingCost.Regions.Europe,
                        Amount = 5m
                    }
                }
            };
            var Basket = new Basket()
            {
                LineItems = new List<LineItem>()
            };
            var lineItem = new LineItem()
            {
                Id = 0,
                DeliveryRegion = RegionShippingCost.Regions.UK,
                Shipping = perRegionDiscountShippingOption,
                SupplierId = 1
            };
            var lineItem_difID = new LineItem()     // There is always difference ID while new lineItem comming
            {
                Id = 1,
                DeliveryRegion = RegionShippingCost.Regions.UK,
                Shipping = perRegionDiscountShippingOption,
                SupplierId = 1
            };
            var lineItem_difRegion = new LineItem()
            {
                Id = 2,
                DeliveryRegion = RegionShippingCost.Regions.Europe,
                Shipping = perRegionDiscountShippingOption,
                SupplierId = 1
            };


            // Check1: the amount should be the same as preDefine
            Basket.LineItems.Add(lineItem);
            var shippingAmount = perRegionDiscountShippingOption.GetAmount(lineItem, Basket);
            Assert.That(shippingAmount, Is.EqualTo(6m));

            // Check2: the amount should have a discount while it's multiple shipping
            Basket.LineItems.Add(lineItem_difID);
            shippingAmount = perRegionDiscountShippingOption.GetAmount(lineItem_difID, Basket);
            Assert.That(shippingAmount, Is.EqualTo(5.5m));

            // Check3: the amount should not have a discount while it's not the same shipping
            Basket.LineItems.Add(lineItem_difRegion);
            shippingAmount = perRegionDiscountShippingOption.GetAmount(lineItem_difRegion, Basket);
            Assert.That(shippingAmount, Is.EqualTo(5m));
        }

        [Test]
        public void BasketShippingTotalTest()
        {
            var perRegionDiscountShippingOption = new PerRegionDiscountShipping()
            {
                SpecialRule_deducte = .5m,
                PerRegionCosts = new[]
                {
                    new RegionShippingCost()
                    {
                        DestinationRegion = RegionShippingCost.Regions.UK,
                        Amount = 6m
                    }
                }
            };

            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.1m};

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                    new LineItem()
                                                        {
                                                            Id = 0,
                                                            DeliveryRegion = RegionShippingCost.Regions.UK,
                                                            Shipping = perRegionDiscountShippingOption,
                                                            SupplierId = 1
                                                        },
                                                    new LineItem()
                                                        {
                                                            Id = 1,
                                                            DeliveryRegion = RegionShippingCost.Regions.UK,
                                                            Shipping = perRegionDiscountShippingOption,
                                                            SupplierId = 1
                                                        },
                                                     new LineItem()
                                                         {
                                                             Id = 2,
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             Id = 3,
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(14.85m));
        }
    }
}