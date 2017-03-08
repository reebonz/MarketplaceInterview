﻿namespace Marketplace.Interview.Business.Shipping
{
    public class NewChoiceShippingCost
    {
        public string DestinationRegion { get; set; }
        public decimal Amount { get; set; }

        public static class Regions
        {
            public const string UK = "UK";
            public const string Europe = "Europe";
            public const string RestOfTheWorld = "RestOfTheWorld";
        }
    }
}