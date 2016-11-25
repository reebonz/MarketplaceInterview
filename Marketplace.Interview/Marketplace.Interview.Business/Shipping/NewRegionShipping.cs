using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class NewRegionShipping : ShippingBase
    {
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            var moreItemsWithSameConds = basket.LineItems
               .Any(
                    element => element.Id != lineItem.Id && element.Shipping is NewRegionShipping
                    && element.SupplierId == lineItem.SupplierId
                    && element.DeliveryRegion == lineItem.DeliveryRegion);
            var amount =
                (from c in PerRegionCosts
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c.Amount).Single();
            var discount = (moreItemsWithSameConds ? Discount : 0);
            return amount - discount;
        }

        public decimal Discount { get; set; }
    }
}