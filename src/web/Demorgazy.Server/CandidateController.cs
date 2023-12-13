using System.Text.Json;
using Demograzy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;


namespace Demograzy.Server
{
    [Route("candidate")]
    public class CandidateController : DemograzyController
    {
        public CandidateController(MainService service) : base(service)
        {
        }


        [HttpGet("{candidateId}/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetName([FromRoute] int candidateId)
        {
            var candidateInfo = await Service.GetCandidateInfo(candidateId);
            return 
                candidateInfo.HasValue ?
                Ok(JsonSerializer.Serialize(candidateInfo.Value.name)) :
                NotFound();
        }

      


    }
}