using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class PerRegionShipping : PerRegionShippingBase
    {
        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }
    }
}