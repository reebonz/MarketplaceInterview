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
    }

    public class LineItem
    {
        public string ProductId { get; set; }
        public decimal Amount { get; set; }
        public int SupplierId { get; set;}
        public ShippingBase Shipping { get; set; }
        public string DeliveryRegion { get; set; }
        public int Id { get; set; }

        private decimal _shippingAmount;

        public decimal ShippingAmount 
        {
            get 
            {
                if (_shippingAmount > default(decimal))
                    return _shippingAmount;
                else
                    return default(decimal);
            }
            set { _shippingAmount = value; }
        }

        public string ShippingDescription { get; set; }
    }
}