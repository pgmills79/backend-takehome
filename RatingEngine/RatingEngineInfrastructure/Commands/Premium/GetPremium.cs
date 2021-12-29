using System;
using System.Linq;
using System.Threading.Tasks;
using RatingEngineCore.HelperClasses;
using RatingEngineCore.Models;


namespace QuoteEngineInfrastructure.Commands.Premium
{
    public static class GetPremium
    {
        private const int HazardFactor = 4;
        public static async Task<RatingEngineCore.Models.Premium> GetPremiumAsync(Payload input)
        {

            var stateFactor = GetStateFactor(input.State);
            var businessFactor = GetBusinessFactor(input.Business);
            var basePremium = GetBasePremium(input.Revenue);
           
            
            var newPremium = new RatingEngineCore.Models.Premium
            {
                PremiumAmount = stateFactor * businessFactor * basePremium * HazardFactor
            };

            return newPremium;

        }

        private static decimal GetBasePremium(decimal revenue)
        {
            return Math.Ceiling(revenue/1000);
        }
        
        private static decimal GetStateFactor(string stateAbbreviation)
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
        
        private static decimal GetBusinessFactor(string businessInput)
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