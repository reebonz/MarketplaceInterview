using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class PerRegionDiscountShipping : ShippingBase
    {
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public decimal SpecialRule_deducte { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0}", lineItem.DeliveryRegion);
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            decimal result = 0m;
            bool isMultipleShipping = false;

            // Multiple rule: "with the same Shipping Option and the same Supplier and Region"
            isMultipleShipping = basket.LineItems.Any(o =>  o.Id != lineItem.Id
                                                            && o.Shipping is PerRegionDiscountShipping
                                                            && o.SupplierId == lineItem.SupplierId
                                                            && o.DeliveryRegion == lineItem.DeliveryRegion);

            // Get Amount by mapping XML
            result = (  from c in PerRegionCosts
                        where c.DestinationRegion == lineItem.DeliveryRegion
                        select c.Amount).Single();

            // Special rule: Get discount
            result = isMultipleShipping ? (result - SpecialRule_deducte) : result;

            return result;
        }
    }
}