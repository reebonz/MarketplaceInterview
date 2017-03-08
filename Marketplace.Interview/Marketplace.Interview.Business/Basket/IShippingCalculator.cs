using Marketplace.Interview.Business.Shipping;
using System.Collections.Generic;
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
            List<LineItem> list = new List<LineItem>();
            int count = 0;
            foreach (var lineItem in basket.LineItems)
            {
                lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
               
                if (count > 0 && count < basket.LineItems.Count)
                {
                    bool supplierMatch = list.Exists(x => x.SupplierId == lineItem.SupplierId);
                    bool regionMatch = list.Exists(x => x.DeliveryRegion == lineItem.DeliveryRegion);
                    bool isNewChoice = lineItem.Shipping is NewChoiceShipping;
                   if (supplierMatch && regionMatch && isNewChoice)
                        lineItem.ShippingAmount = lineItem.ShippingAmount - (decimal)0.5;
                }
                if (lineItem.Shipping is NewChoiceShipping)
                {
                    list.Add(lineItem);
                    count++;
                }
            }

            return basket.LineItems.Sum(li => li.ShippingAmount);
        }
    }
}