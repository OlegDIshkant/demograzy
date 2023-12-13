using Demograzy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;


namespace Demograzy.Server
{
    [Route("")]
    public class EntryPointController : DemograzyController
    {
        public EntryPointController(MainService service) : base(service)
        {
        }



        [HttpGet]
        public IActionResult GetMainPage()
        {
            return Redirect(GenRedirectUrlToMainPage(Request));
        }


        private string GenRedirectUrlToMainPage(HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host}/index.html";
        }

    }
}