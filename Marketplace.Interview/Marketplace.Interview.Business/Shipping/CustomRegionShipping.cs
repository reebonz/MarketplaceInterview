using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class CustomRegionShipping : ShippingBase
    {
        public decimal DiscountValue { get; set; }
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Custom Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {

            var shouldReduce = basket.LineItems
            .Any(item => item.Id != lineItem.Id
                         && item.Shipping is CustomRegionShipping
                         && item.SupplierId == lineItem.SupplierId
                         && item.DeliveryRegion == lineItem.DeliveryRegion);
            return
                (from c in PerRegionCosts
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c.Amount).Single() - (shouldReduce ? DiscountValue : 0); ;
        }


    }
}
