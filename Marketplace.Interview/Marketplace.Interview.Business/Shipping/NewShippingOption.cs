using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class NewShippingOption : ShippingBase
    {
        public decimal Discount { get; set; }
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            var conditions = basket.LineItems
                .Any(
                    x => x.Id != lineItem.Id && x.Shipping is NewShippingOption
                         && x.SupplierId == lineItem.SupplierId
                         && x.DeliveryRegion == lineItem.DeliveryRegion);

            var discount = conditions ? Discount : 0;

            var amount =
                PerRegionCosts.Where(x => x.DestinationRegion == lineItem.DeliveryRegion).Select(c => c.Amount).Single();

            return amount - discount;
        }
    }
}