﻿using System;
using Marketplace.Interview.Business.Basket;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Interview.Business.Shipping
{
    public class ConditionalPerRegionShipping : ShippingBase
    {

        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("ConditionalPerRegion Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetDiscount
        {
            get { return .5m; }
            set { }
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {

            decimal amt = (from c in PerRegionCosts
                           where c.DestinationRegion == lineItem.DeliveryRegion
                           select c.Amount).Single();
            /*
            int numMatched = (from bl in basket.LineItems
                              where bl.Shipping == lineItem.Shipping && bl.SupplierId == lineItem.SupplierId && bl.DeliveryRegion == lineItem.DeliveryRegion
                              select bl).Count();
                              */
            //int numMatched = basket.LineItems.Count(bl => bl.ShippingDescription == lineItem.ShippingDescription && bl.SupplierId == lineItem.SupplierId && bl.DeliveryRegion == lineItem.DeliveryRegion);

            //if (numMatched > 1)
            //    return amt - (decimal)0.5;
            //else
            //    return amt;

            return amt;
        }
    }
}
