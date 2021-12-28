using System.Threading.Tasks;
using RatingEngineCore.Models;


namespace QuoteEngineInfrastructure.Commands.Premium
{
    public class GetPayloadPremium
    {
        private static string PremiumAmount { get; set; }
        public static async Task<string> GetPremiumAsync(Payload premium)
        {
            PremiumAmount = "1000";
            
            return PremiumAmount;

        }
        
       

    }
}