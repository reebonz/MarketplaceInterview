using Marketplace.Interview.Business.Constant;
using Marketplace.Interview.Business.Security;
using System;
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
                int ret = (from c in basket.LineItems
                           where c.Shipping.GetType() == lineItem.Shipping.GetType()
                           && c.SupplierId == lineItem.SupplierId
                           && c.DeliveryRegion == lineItem.DeliveryRegion
                           && c.Id<lineItem.Id
                           select c).Count();

                if (ret > default(int))
                {
                    lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket) - Convert.ToDecimal(AppConfigs.getShippingDiscount);
                }
                else
                {
                    lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                }

                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }

            return basket.LineItems.Sum(li => li.ShippingAmount);
        }
    }
}