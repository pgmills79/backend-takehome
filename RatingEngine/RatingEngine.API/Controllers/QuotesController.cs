using System.Net.Mime;
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
    public class QuotesController : ControllerBase
    {

        public readonly IActionHandler _actionHandler;

        public QuotesController (IActionHandler actionHandler) 
        {
            _actionHandler = actionHandler;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RatingEngineCore.Models.Premium>> CreatePremiumAmount([FromBody] Payload payloadInput)
        {
            var results = await _actionHandler.Premiums.ReturnPremiumAmount(payloadInput);
            return results;
        }
    }
}