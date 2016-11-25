using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class DiscountShipping : PerRegionShipping
    {
        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {

            int counter = 0;
            foreach (LineItem l in basket.LineItems)
            {
                if (l.Shipping is DiscountShipping
                    && l.SupplierId == lineItem.SupplierId
                    && l.DeliveryRegion.Equals(lineItem.DeliveryRegion))
                {
                    counter++;
                }
            }

           return base.GetAmount(lineItem, basket) - (counter > 1 ? .5m : 0);
            
        }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return String.Format("DiscountShipping {0} - {1} - {2}", lineItem.SupplierId, lineItem.DeliveryRegion, lineItem.ShippingDescription);
        }
    }
}
