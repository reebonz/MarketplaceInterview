using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class DiscountedPerRegionShipping : PerRegionShippingBase
    {
        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Discounted Shipping to {0}", lineItem.DeliveryRegion);
        }

        public static decimal Discount { get { return .5m; } }
    }
}
