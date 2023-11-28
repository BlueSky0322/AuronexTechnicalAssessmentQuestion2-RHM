using Microsoft.AspNetCore.Mvc;
using RHM.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace RHM.API.Controllers
{
    [Route("Hasher")]
    [ApiController]
    public class EndpointsController : ControllerBase
    {
        private static Random random = new Random();
        private readonly HttpClient _httpClient;
        private readonly ILogger<EndpointsController> _logger;

        public EndpointsController(HttpClient httpClient, ILogger<EndpointsController> logger)
        {
            _httpClient = httpClient;
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
                _logger.LogInformation($"[API][GenerateNewHash] Received GET request with content: {hashedString}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

        }

        //Endpoint 2
        [HttpGet("ValidateHash")]
        public async Task<ActionResult<Endpoint2Response>> ValidateHash()
        {
            try
            {
                //Local variables for storing certain values
                bool hashPassed = false;
                char lastChar;
                string retrievedHash;
                int attempts = 0;

                //Create response object.
                var endpoint2Response = new Endpoint2Response
                {
                    Hash = null,
                    ResponseMessage = "No hash string was received.",
                    Attempts = attempts,
                };

                while (!hashPassed)
                {
                    //Initiate attempt counter
                    attempts++;

                    //Make HTTP request to Endpoint 1 to get hash string
                    var endpoint1Response = await _httpClient.GetAsync("https://localhost:7057/Hasher/GenerateNewHash");
                    if (endpoint1Response.IsSuccessStatusCode)
                    {
                        //Retrieve and read the content as JSON object
                        var content = await endpoint1Response.Content.ReadFromJsonAsync<Endpoint1Response>();
                        if (content != null && content.Hash != null)
                        {
                            _logger.LogInformation($"[API][ValidateHash] Hash: {content.Hash}");
                            //Assign retrieved value to local variable
                            retrievedHash = content.Hash;

                            //Assign local variable to response object property
                            endpoint2Response.Hash = retrievedHash;

                            //Extract the last character from the hash string
                            lastChar = retrievedHash.Last();

                            //Validate if last character is number && odd number
                            if (char.IsDigit(lastChar))
                            {
                                //Validate if last character is odd number
                                int lastNum = int.Parse(lastChar.ToString());
                                if (lastNum % 2 != 0)
                                {
                                    //Break out of the loop only if conditions met
                                    hashPassed = true;
                                    endpoint2Response.ResponseMessage = $"The last character is '{lastChar}'. This is a number and odd number. Pass!";
                                    endpoint2Response.Attempts = attempts;

                                    _logger.LogInformation($"[API][ValidateHash] {endpoint2Response.ResponseMessage}");
                                    break;
                                }
                                else
                                {
                                    //Even number
                                    endpoint2Response.ResponseMessage = $"The last character is '{lastChar}'. This is an even number. Does not pass.";
                                    endpoint2Response.Attempts = attempts;
                                    _logger.LogInformation($"[API][ValidateHash] {endpoint2Response.ResponseMessage}");
                                }
                            }
                            else
                            {
                                //Alphabet
                                endpoint2Response.ResponseMessage = $"The last character is '{lastChar}'. This is an alphabet. Does not pass.";
                                endpoint2Response.Attempts = attempts;
                                _logger.LogInformation($"[API][ValidateHash] {endpoint2Response.ResponseMessage}");
                            }
                        }                      
                    }

                }

                //Success response body only if conditions are met
                if (hashPassed)
                {
                    _logger.LogInformation($"[API][ValidateHash] Hash passed in ({attempts}) attempts.");
                    return Ok(endpoint2Response);
                }
                else
                {
                    return NotFound(endpoint2Response);
                }
            }
            catch (Exception ex)
            {
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
