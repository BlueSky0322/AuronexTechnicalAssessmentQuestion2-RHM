using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace RHM.API.Controllers
{
    [Route("Hasher")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private static Random random = new Random();

        private readonly ILogger<EndpointsController> _logger;

        public EndpointsController(ILogger<EndpointsController> logger)
        {
            _logger = logger;
        }

        //Endpoint 1
        [HttpGet("GenerateNewHash")]
        public async Task<ActionResult<string>> GenerateHash()
        {
            await Task.Delay(100);

            return Ok();
        }

        //Generate a random string 
        //ref:https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        private string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 5) //arbitrary length of characters
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Hash the random string into random string hash using SHA-256

    }
}
