using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Business.Basket
{
    public class Basket
    {
        public List<LineItem> LineItems { get; set; }
        public decimal Shipping { get; set; }

        public int ItemCountWith(ShippingBase shippingType, string DeliveryRegion, int SupplierId)
        {
            var count =  (from li in LineItems
                    where li.DeliveryRegion == DeliveryRegion
                    && li.Shipping.GetType() == shippingType.GetType()
                    && li.SupplierId == SupplierId
                    select li.Amount).Count();

            return count;
        }
    }

    public class LineItem
    {
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
        public int SupplierId { get; set;}
        public ShippingBase Shipping { get; set; }
        public string DeliveryRegion { get; set; }
        public int Id { get; set; }

        public decimal ShippingAmount { get; set; }

        public string ShippingDescription { get; set; }
    }
}