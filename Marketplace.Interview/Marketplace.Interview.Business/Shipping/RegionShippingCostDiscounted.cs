namespace Marketplace.Interview.Business.Shipping
{
    public class RegionShippingCostDiscounted : RegionShippingCost
    {
        public int MinSameOriginItems { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}