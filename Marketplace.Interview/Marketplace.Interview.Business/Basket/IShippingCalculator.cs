using Marketplace.Interview.Business.Shipping;
using System.Linq;

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
            foreach (var lineItem in basket.LineItems)
            {
                lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }

            var sum = basket.LineItems.Sum(li => li.ShippingAmount);

            var discountedLineItems = from lineItem in basket.LineItems
                       where lineItem.Shipping is DiscountedPerRegionShipping
                       group lineItem by new { lineItem.DeliveryRegion, lineItem.SupplierId }
                       into similarLineItems
                       where similarLineItems.Count() > 1
                       select similarLineItems;
           var discountedSum = sum - DiscountedPerRegionShipping.Discount * discountedLineItems.Count();

            return discountedSum; 
        }
    }
}