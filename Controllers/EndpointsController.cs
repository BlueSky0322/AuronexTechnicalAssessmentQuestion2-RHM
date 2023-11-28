using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace RHM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private readonly ILogger<EndpointsController> _logger;

        public EndpointsController(ILogger<EndpointsController> logger)
        {
            _logger = logger;
        }
    }
}
