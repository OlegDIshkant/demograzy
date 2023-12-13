using System.Text.Json;
using Demograzy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;


namespace Demograzy.Server
{
    [Route("client")]
    public class ClientController : DemograzyController
    {
        public ClientController(MainService service) : base(service)
        {
        }


        [HttpPut("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewClient([FromQuery] string name)
        {
            var clientId = await Service.AddClientAsync(name);
            return Created("", clientId);
        }



        [HttpPut("{clientId}/room/new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewRoom([FromRoute] int clientId, [FromQuery] string title, [FromHeader] string passphrase)
        {
            var roomId = await Service.AddRoomAsync(clientId, title, passphrase);            
            return
                roomId.HasValue ?
                Created("", JsonSerializer.Serialize(roomId)) :
                BadRequest();
        }



        [HttpPut("{clientId}/name")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClientName([FromRoute] int clientId, [FromQuery] string title, [FromHeader] string passphrase)
        {            
            var clientInfo = await Service.GetClientInfo(clientId);
            return 
                clientInfo.HasValue ?
                Ok(clientInfo.Value.name) :
                NotFound();            
        }
        


        


    }
}