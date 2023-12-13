using System.Text.Json;
using Demograzy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;


namespace Demograzy.Server
{
    [Route("room")]
    public class RoomController : DemograzyController
    {
        public RoomController(MainService service) : base(service)
        {
        }



        [HttpGet("{roomId}/title")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTitle([FromRoute] int roomId)
        {
            var roomInfo = await Service.GetRoomInfoAsync(roomId);
            return
                roomInfo.HasValue ?
                Ok(JsonSerializer.Serialize(roomInfo.Value.title)) :
                NotFound();
        }



        [HttpGet("{roomId}/voting_started")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> IsVotingStarted([FromRoute] int roomId)
        {
            var roomInfo = await Service.GetRoomInfoAsync(roomId);
            return
                roomInfo.HasValue ?
                Ok(JsonSerializer.Serialize(roomInfo.Value.votingStarted)) :
                NotFound();
        }



        [HttpPost("{roomId}/start_voting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StartVoting([FromRoute] int roomId)
        {
            return 
                (await Service.StartVotingAsync(roomId)) ?
                Ok() :
                BadRequest();
        }



        [HttpGet("{roomId}/members")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMembers([FromRoute] int roomId)
        {
            var memberIds = await Service.GetMembers(roomId);
            return
                memberIds != null ?
                Ok(JsonSerializer.Serialize(memberIds)) :
                NotFound();
        }



        [HttpPut("{roomId}/members/new")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMember(
            [FromRoute] int roomId,
            [FromQuery] int memberId,
            [FromHeader] string passphrase)
        {
            return 
                (await Service.AddMember(roomId, memberId, passphrase)) ?
                Ok() :
                BadRequest();
        }



        [HttpGet("{roomId}/candidates")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCandidates([FromRoute] int roomId)
        {
            var candidateIds = await Service.GetCandidatesAsync(roomId);
            return
                candidateIds != null ?
                Ok(JsonSerializer.Serialize(candidateIds)) :
                NotFound();
        }



        [HttpPut("{roomId}/candidates/new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCandidate([FromRoute] int roomId, [FromQuery] string name)
        {
            var candidateId = await Service.AddCandidateAsync(roomId, name);
            return
                candidateId.HasValue ?
                Created("", JsonSerializer.Serialize(candidateId.Value)) :
                BadRequest();
        }



        [HttpGet("{roomId}/winner")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWinner([FromRoute] int roomId)
        {
            var winnerId = await Service.GetWinnerAsync(roomId);
            return
                winnerId.HasValue ?
                Ok(JsonSerializer.Serialize(winnerId.Value)) :
                NotFound();
        }



        [HttpGet("{roomId}/members/{clientId}/active_verses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetWinner(
            [FromRoute] int roomId,
            [FromRoute] int clientId)
        {
            var versesIds = await Service.GetActiveVersesAsync(roomId, clientId);
            return
                versesIds != null ?
                Ok(JsonSerializer.Serialize(versesIds)) :
                NotFound();
        }


    }
}