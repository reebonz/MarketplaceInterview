using System.Collections.Generic;
using System.Linq;
using Marketplace.Interview.Business.Basket;

namespace Marketplace.Interview.Business.Shipping
{
    public class NewChoiceShipping : ShippingBase
    {
        public IEnumerable<NewChoiceShippingCost> NewChoiceCosts { get; set; }

        public override string GetDescription(LineItem lineItem, Basket.Basket basket)
        {
            return string.Format("New choice for Shipping ");
        }

        public override decimal GetAmount(LineItem lineItem, Basket.Basket basket)
        {
            return
                (from c in NewChoiceCosts
                 where c.DestinationRegion == lineItem.DeliveryRegion
                 select c.Amount).Single();
        }
    }
}