using QuoteEngineInfrastructure.Commands.Premium;

namespace QuoteEngineInfrastructure
{
    public interface IActionHandler
    {
        GetPayloadPremium Premium { get; }
        
    }

    public class ActionHandler : IActionHandler
    {
        public ActionHandler(GetPayloadPremium premium)
        {
            Premium = premium;
        }

        public GetPayloadPremium Premium { get; }
    }
}