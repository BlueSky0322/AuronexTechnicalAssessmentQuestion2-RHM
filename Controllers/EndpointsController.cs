using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RHM.API.Models;
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
        public async Task<ActionResult<Endpoint1Response>> GenerateHash()
        {
            try
            {
                //Delay by 1 second (1000 milliseconds)
                await Task.Delay(1000);

                //Generate random string and hash it
                string generatedString = GenerateRandomString();
                string hashedString = SHA256Hasher(generatedString);

                //Create response object.
                var response = new Endpoint1Response
                {
                    Hash = hashedString,
                };

                //Logging for debug purposes
                _logger.LogInformation($"[GenerateNewHash] Received GET request with content: {hashedString}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        //Endpoint 2
        [HttpGet("ValidateHash")]
        public async Task<ActionResult<Endpoint2Response>> ValidateHash()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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
            //Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(generatedString));

                //Convert byte array to a string
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
