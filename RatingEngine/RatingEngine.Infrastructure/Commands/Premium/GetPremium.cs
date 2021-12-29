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

            var stateFactor = Factors.GetStateFactor(input.State);
            var businessFactor = Factors.GetBusinessFactor(input.Business);
            var basePremium = Factors.GetBasePremium(input.Revenue);
           
            
            var newPremium = new RatingEngineCore.Models.Premium
            {
                PremiumAmount = stateFactor * businessFactor * basePremium * HazardFactor
            };

            return newPremium;

        }
    }
}