using Marketplace.Interview.Business.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Shipping
{
    public abstract class PerRegionShippingBase : ShippingBase
    {
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            return
                (from c in PerRegionCosts
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c.Amount).Single();
        }
    }
}
