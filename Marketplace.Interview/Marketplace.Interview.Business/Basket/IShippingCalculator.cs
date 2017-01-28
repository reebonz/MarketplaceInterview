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
                //Added by Kenny Loo
                decimal deductAmt = (decimal)0.5;

                var duplicate = from d in basket.LineItems
                                where d.DeliveryRegion == lineItem.DeliveryRegion && d.SupplierId == lineItem.SupplierId && d.Shipping.GetType().Name.Equals(lineItem.Shipping.GetType().Name.ToString())
                                group d by new { d.Shipping, d.SupplierId, d.DeliveryRegion }
                                    into g
                                    select g;

                if (duplicate.Count() > 1)
                {
                    lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket) - deductAmt;
                }
                else
                {
                    lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                }
                // End
               
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }

            return basket.LineItems.Sum(li => li.ShippingAmount);
        }
    }
}