using QuoteEngineInfrastructure.Commands.Premium;
namespace QuoteEngineInfrastructure
{
    public interface IActionHandler
    {
        IPremium Premiums { get; }
    }

    public class ActionHandler : IActionHandler
    {
        public ActionHandler(IPremium premiums)
        {
            Premiums = premiums;
        }

        public IPremium Premiums { get; set; }

    } 
 }
