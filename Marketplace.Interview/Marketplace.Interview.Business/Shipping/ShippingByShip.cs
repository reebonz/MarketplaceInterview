//Added by Kenny
using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;
namespace Marketplace.Interview.Business.Shipping
{
    public class ShippingByShip : ShippingBase
    {
        public IEnumerable<RegionShippingCost> ShippingByShipCost { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format(" Shipping to {0} by Ship", lineItem.DeliveryRegion);
        }
        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            return
            (from c in ShippingByShipCost
             where c.DestinationRegion == lineItem.DeliveryRegion
             select c.Amount).Single();
        }
    }
}
//End