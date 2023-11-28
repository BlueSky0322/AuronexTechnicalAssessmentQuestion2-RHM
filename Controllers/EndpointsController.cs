using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;

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

        //Generate a random string using LINQ
        //ref:https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
        private string GenerateRandomString()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"; //all possible alphanumerical char
            return new string(Enumerable.Repeat(chars, 5) //arbitrary length of characters
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //Hash the random string into random string hash using SHA-256
        //ref:https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/
        private string SHA256Hasher(string generatedString)
        {
            // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(generatedString));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
