using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class PerRegionDiscountShipping : ShippingBase
    {
        public IEnumerable<RegionShippingCost> PerRegionCosts { get; set; }

        public decimal SpecialRule_deducte { get; set; }

        private bool isMultipleShipping { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("Shipping to {0} {1}"
                                , lineItem.DeliveryRegion
                                , isMultipleShipping ? "with discount" : "");
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            decimal result = 0m;
            List<LineItem> foundItemList;
            isMultipleShipping = false;

            #region Multiple rule: "if there is at least one other item in the basket with the same Shipping Option and the same Supplier and Region" 

            foundItemList = basket.LineItems.FindAll(o =>  o.Shipping is PerRegionDiscountShipping
                                                            && o.SupplierId == lineItem.SupplierId
                                                            && o.DeliveryRegion == lineItem.DeliveryRegion);

            // "at least one other item" && it should start to get a discount at the second item.
            if (foundItemList.Count >= 2 && lineItem.Id != foundItemList[0].Id)
            {
                isMultipleShipping = true;
            }
            #endregion

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