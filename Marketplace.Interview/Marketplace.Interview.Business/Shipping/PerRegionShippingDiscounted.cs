using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;
using System;

namespace Marketplace.Interview.Business.Shipping
{
    public class PerRegionShippingDiscounted : ShippingBase
    {
        public IEnumerable<RegionShippingCostDiscounted> PerRegionCostsDiscounted { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            var shippingOption = (from c in PerRegionCostsDiscounted
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c).Single();
            var cost = shippingOption.Amount;
            var count = basket.ItemCountWith(this, lineItem.DeliveryRegion, lineItem.SupplierId);
            if (count >= shippingOption.MinSameOriginItems)
            {
                cost = Math.Max(cost - shippingOption.DiscountAmount, 0);
            }
            return cost;
        }
    }
}