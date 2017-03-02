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
        public void BasketShippingTotalTest()
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

            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.1m};

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(3.35m));
        }

        [Test]
        public void NewShippingOptionTest_Scenario_ShouldDeductedShipping()
        {
            var newShippingOption = NewShippingOption_InitArrange();

            var basket = new Basket
            {
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Id = 1,
                        SupplierId = 1,
                        DeliveryRegion = RegionShippingCost.Regions.Europe,
                        Shipping = newShippingOption
                    },
                    new LineItem
                    {
                        Id = 2,
                        SupplierId = 1,
                        DeliveryRegion = RegionShippingCost.Regions.Europe,
                        Shipping = newShippingOption
                    }
                }
            };

            var shippingAmount = newShippingOption.GetAmount(basket.LineItems[0], basket);
            Assert.That(shippingAmount, Is.EqualTo(.5m));

            shippingAmount = newShippingOption.GetAmount(basket.LineItems[1], basket);
            Assert.That(shippingAmount, Is.EqualTo(.5m));
        }

        [Test]
        public void NewShippingOptionTest_Scenario_ShouldNotDeductedShipping()
        {
            var newShippingOption = NewShippingOption_InitArrange();

            var basket = new Basket
            {
                LineItems = new List<LineItem>
                {
                    new LineItem
                    {
                        Id = 1,
                        SupplierId = 1,
                        DeliveryRegion = RegionShippingCost.Regions.UK,
                        Shipping = newShippingOption
                    },
                    new LineItem
                    {
                        Id = 2,
                        SupplierId = 1,
                        DeliveryRegion = RegionShippingCost.Regions.Europe,
                        Shipping = newShippingOption
                    }
                }
            };

            var shippingAmount = newShippingOption.GetAmount(basket.LineItems[0], basket);
            Assert.That(shippingAmount, Is.EqualTo(0.5m));

            shippingAmount = newShippingOption.GetAmount(basket.LineItems[1], basket);
            Assert.That(shippingAmount, Is.EqualTo(1m));
        }

        private static NewShippingOption NewShippingOption_InitArrange()
        {
            var newShippingOption = new NewShippingOption
            {
                PerRegionCosts = new[]
                {
                    new RegionShippingCost
                    {
                        DestinationRegion = RegionShippingCost.Regions.UK,
                        Amount = 0.5m
                    },
                    new RegionShippingCost
                    {
                        DestinationRegion = RegionShippingCost.Regions.Europe,
                        Amount = 1m
                    }
                },
                Discount = 0.5m
            };

            return newShippingOption;
        }
    }
}