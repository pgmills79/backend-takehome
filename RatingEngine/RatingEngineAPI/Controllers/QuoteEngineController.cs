using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteEngineInfrastructure;
using QuoteEngineInfrastructure.Commands.Premium;
using RatingEngineCore.Models;


namespace RatingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteEngineController : ControllerBase
    {
        private readonly IActionHandler _actionHandler;

        public QuoteEngineController(IActionHandler actionHandler)
        {
            _actionHandler = actionHandler;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Payload))]
        public async Task<string> GetPremium([FromBody] Payload premium)
        {
            var results = await GetPayloadPremium.GetPremiumAsync(premium);
            return results;
        }
    }
}