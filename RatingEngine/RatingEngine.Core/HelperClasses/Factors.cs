using System;
using System.Linq;

namespace RatingEngineCore.HelperClasses
{
    public static class Factors
    {
        
        public static decimal GetBasePremium(decimal revenue)
        {
            return Math.Ceiling(revenue/1000);
        }
        
        public static decimal GetStateFactor(string stateAbbreviation)
        {
            var stateFactorPairs = new KeyValueList<string, decimal>
            {
                { "OH", 1m },
                { "FL", 1.2m },
                { "TX", 0.943m },
            };

            var factor = stateFactorPairs
                .Where(x => x.Key.Equals(stateAbbreviation))
                .Select(x => x.Value).FirstOrDefault();

            return factor;
        }
        
        public static decimal GetBusinessFactor(string businessInput)
        {
            var businessFactors = new KeyValueList<string, decimal>
            {
                { "Architect", 1m },
                { "Plumber", 0.5m },
                { "Programmer", 1.25m },
            };

            var factor = businessFactors
                .Where(x => x.Key.Equals(businessInput))
                .Select(x => x.Value).FirstOrDefault();

            return factor;
        }
        
    }
}