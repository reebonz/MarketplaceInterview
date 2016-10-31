﻿using System;
using Marketplace.Interview.Business.Basket;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Interview.Business.Shipping
{
    public class ConditionalPerRegionShipping : ShippingBase
    {

        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }
        //public decimal Discount { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("ConditionalPerRegion Shipping to {0}", lineItem.DeliveryRegion);
        }

        //public override decimal GetDiscount
        //{
        //    get { return .5m; }
        //    set { }
        //}

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {

            decimal amt = (from c in PerRegionCosts
                           where c.DestinationRegion == lineItem.DeliveryRegion
                           select c.Amount).Single();

            var discountFlag = false;
            /*
            int numMatched = (from bl in basket.LineItems
                              where bl.Shipping == lineItem.Shipping && bl.SupplierId == lineItem.SupplierId && bl.DeliveryRegion == lineItem.DeliveryRegion
                              select bl).Count();
                              */
            var numMatched = basket.LineItems.Where(bl=>bl.SupplierId == lineItem.SupplierId && bl.DeliveryRegion == lineItem.DeliveryRegion).ToList();

            if (numMatched.Count > 1 && numMatched != null)
            {
                if (lineItem != numMatched[0])
                {
                    discountFlag = true;
                }
            }

            return discountFlag ? amt - (decimal)0.5 : amt;
        }
    }
}
