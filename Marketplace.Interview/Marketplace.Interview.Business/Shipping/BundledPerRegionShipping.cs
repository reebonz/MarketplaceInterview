using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class BundledPerRegionShipping : BasePerRegionShipping
    {
        public static decimal DiscountAmount = 0.5M;
    }
}