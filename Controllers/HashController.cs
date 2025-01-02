using Microsoft.AspNetCore.Mvc;
using HashProcessorApp.Services;
using HashProcessorApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HashProcessorApp.Data;
using System.Security.Cryptography;
using System.Text;
using EasyNetQ;

namespace HashProcessorApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashController : ControllerBase
    {
        private readonly HashProcessorService _hashProcessorService;
        private readonly HashesDbContext _dbContext; // Inject DbContext here

        // Inject HashProcessorService and HashesDbContext into controller
        public HashController(HashProcessorService hashProcessorService, HashesDbContext dbContext)
        {
            _hashProcessorService = hashProcessorService;
            _dbContext = dbContext; // Initialize _dbContext
        }

      // Endpoint POST '/hashes' will generate 40,000 random SHA1 hashes 
// and send them one by one to RabbitMQ queue for further processing


// Endpoint POST '/hashes' will generate 40,000 random SHA1 hashes 
// and send them one by one to RabbitMQ queue for further processing
    [HttpPost("hashes")]
    public async Task<IActionResult> GenerateAndSendHashes()
    {
    try
    {
        // Generate 40,000 random SHA1 hashes
        var hashes = GenerateRandomHashes(40000);

        // Send each hash to the RabbitMQ queue using EasyNetQ
        foreach (var hash in hashes)
        {
            await SendMessageToRabbitMQ(hash);
        }

        return Ok("40,000 random SHA1 hashes generated and sent to RabbitMQ queue.");
    }
    catch (Exception ex)
    {
        // Log the exception if needed
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}

// Helper method to generate random SHA1 hashes
private List<string> GenerateRandomHashes(int count)
{
    var hashes = new List<string>();
    using (var sha1 = SHA1.Create())
    {
        for (int i = 0; i < count; i++)
        {
            var randomData = Guid.NewGuid().ToString(); // Using GUID as random data
            var hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(randomData));
            var hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty); // Convert bytes to a hex string
            hashes.Add(hashString);
        }
    }
    return hashes;
}

// Method to send message to RabbitMQ using EasyNetQ
private async Task SendMessageToRabbitMQ(string hash)
{
    // Create a connection to RabbitMQ using EasyNetQ
    var bus = RabbitHutch.CreateBus("amqp://guest:guest@localhost:5672");

    // Send the hash to the RabbitMQ queue (using "hashes" as the queue name)
    await bus.PubSub.PublishAsync(hash, "hashes");

    // Optionally, you can log that the message was sent.
    Console.WriteLine($"Sent hash to RabbitMQ: {hash}");
}


        // GET: api/hash
[HttpGet]
public async Task<ActionResult<IEnumerable<object>>> GetHashCountsGroupedByDay()
{
    try
    {
        // Group the hashes by day and count the occurrences for each day
        var hashCountsGroupedByDay = await _dbContext.HashCounts
            .GroupBy(h => h.Date.Date)  // Group by the date (ignoring time)
            .Select(group => new
            {
                Date = group.Key,  // The date of the group
                Count = group.Count()  // The number of hashes for that day
            })
            .ToListAsync();

        // Return the grouped data as JSON
        return Ok(hashCountsGroupedByDay);
    }
    catch (Exception ex)
    {
        // Return a server error if something goes wrong
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}


        // GET: api/hash/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<HashCount>> GetHashCountById(int id)
        {
            var hashCount = await _dbContext.HashCounts.FindAsync(id); // Use _dbContext here
            if (hashCount == null)
            {
                return NotFound();
            }
            return Ok(hashCount); // Returns the specific hash count
        }
    }
}
