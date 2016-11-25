using System.Linq;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Business.Basket
{
    public interface IShippingCalculator
    {
        decimal CalculateShipping(Basket basket);
    }

    public class ShippingCalculator : IShippingCalculator
    {
        public decimal CalculateShipping(Basket basket)
        {
            var discountAmount = BundledPerRegionShipping.DiscountAmount;
            foreach (var lineItem in basket.LineItems)
            {
                lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }

            var total = basket.LineItems.Sum(li => li.ShippingAmount);

            var groupsContainesManyItems = from tBasket in basket.LineItems
                                           where tBasket.Shipping is BundledPerRegionShipping
                                           group basket by new { tBasket.DeliveryRegion, tBasket.SupplierId }
                                           into tData
                                           where tData.Count() > 1
                                           select tData;
            if(groupsContainesManyItems.Any())
            {
                foreach(var tBasket in groupsContainesManyItems)
                {
                    total -= discountAmount;
                }
            }
            return total;
        }
    }
}