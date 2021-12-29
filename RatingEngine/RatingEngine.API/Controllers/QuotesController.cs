using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuoteEngineInfrastructure.Commands.Premium;
using RatingEngineCore.Models;


namespace RatingEngine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Premium>> CreatePremiumAmount([FromBody] Payload payloadInput)
        {
            var results = await GetPremium.ReturnPremiumAmountAsync(payloadInput);
            return results;
        }
    }
}