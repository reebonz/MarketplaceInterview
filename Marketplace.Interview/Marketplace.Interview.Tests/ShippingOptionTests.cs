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
        //HuyDP
        [Test]
        public void NewChoiceShippingOptionTest()
        {
            var newChoiceShippingOption = new NewChoiceShipping()
            {
                NewChoiceCosts = new[]
                                                                       {
                                                                           new NewChoiceShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       NewChoiceShippingCost.Regions.UK,
                                                                                   Amount = .5m
                                                                               },
                                                                           new NewChoiceShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       NewChoiceShippingCost.Regions.Europe,
                                                                                   Amount = 1.0m
                                                                               }
                                                                       },
            };

            var shippingAmount = newChoiceShippingOption.GetAmount(new LineItem() { DeliveryRegion = NewChoiceShippingCost.Regions.Europe }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.0m));

            shippingAmount = newChoiceShippingOption.GetAmount(new LineItem() { DeliveryRegion = NewChoiceShippingCost.Regions.UK }, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(0.5m));
        }

        //End-HuyDP

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
                                                                                   Amount =0.5m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.0m
                                                                               }
                                                                       },
            };
            //HuyDP
            var newChoiceShippingOption = new NewChoiceShipping()
            {
                NewChoiceCosts = new[]
                                                                       {
                                                                           new NewChoiceShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       NewChoiceShippingCost.Regions.UK,
                                                                                   Amount = .5m
                                                                               },
                                                                           new NewChoiceShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       NewChoiceShippingCost.Regions.Europe,
                                                                                   Amount = 1.0m
                                                                               }
                                                                       },
            };
            //End-HuyDP

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
                                                     //HuyDP
                                                    new LineItem()
                                                         {
                                                             DeliveryRegion = NewChoiceShippingCost.Regions.UK,
                                                             Shipping = newChoiceShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = NewChoiceShippingCost.Regions.Europe,
                                                             Shipping = newChoiceShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = NewChoiceShippingCost.Regions.Europe,
                                                             Shipping = newChoiceShippingOption
                                                         },
                                                     //End-HuyDP
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(4.6m));
        }
    }
}