using System.Threading.Tasks;
using RatingEngineCore.HelperClasses;
using RatingEngineCore.Models;


namespace QuoteEngineInfrastructure.Commands.Premium
{

    public interface IPremium 
    {
        public Task<RatingEngineCore.Models.Premium> ReturnPremiumAmount(Payload input);
    }
    public class Premium : IPremium
    {
        private const int HazardFactor = 4;
        public Task<RatingEngineCore.Models.Premium> ReturnPremiumAmount(Payload input)
        {

            var stateFactor = Factors.GetStateFactor(input.State);
            var businessFactor = Factors.GetBusinessFactor(input.Business);
            var basePremium = Factors.GetBasePremium(input.Revenue);
           
            
            var newPremium = new RatingEngineCore.Models.Premium
            {
                PremiumAmount = stateFactor * businessFactor * basePremium * HazardFactor
            };

            return Task.FromResult(newPremium);

        }
    }
}