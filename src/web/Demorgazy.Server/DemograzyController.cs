using Demograzy.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace Demograzy.Server
{
    [ApiController]
    public class DemograzyController : ControllerBase
    {
        protected MainService Service { get; private set; }

        public DemograzyController(MainService service)
        {
            Service = service;
        }

    }
}