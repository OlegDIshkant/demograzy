using System.Text.Json;
using Demograzy.BusinessLogic;
using Demograzy.BusinessLogic.DataAccess;
using Microsoft.AspNetCore.Mvc;


namespace Demograzy.Server
{
    [Route("versus")]
    public class VersusController : DemograzyController
    {
        public VersusController(MainService service) : base(service)
        {
        }



        [HttpGet("{versusId}/candidates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCandidates([FromRoute] int versusId)
        {
            var versusInfo = await Service.GetVersusInfoAsync(versusId);
            return 
                versusInfo.HasValue ?
                Ok(JsonSerializer.Serialize(ExtractCandidates(versusInfo.Value))) :
                NotFound();
        }



        private List<int> ExtractCandidates(VersusInfo info)
        {
            return new List<int>()
            {
                info.firstCandidateId,
                info.secondCandidateId
            };
        }



        [HttpPost("{versusId}/vote")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCandidates(
            [FromRoute] int versusId,
            [FromQuery] int voter,
            [FromQuery] bool voteForFirst)
        {
            return 
                (await Service.VoteAsync(versusId, voter, voteForFirst)) ?
                Ok() :
                NotFound();
        }
       


    }
}