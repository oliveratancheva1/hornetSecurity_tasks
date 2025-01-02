using HashProcessorApp.Services;
using System.Threading.Tasks;

namespace HashProcessorApp.Services
{
    public class HashProcessorService
    {
        private readonly RabbitMQService _rabbitMQService;

        // Constructor to inject RabbitMQService
        public HashProcessorService(RabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        // Your business logic, for example:
        public async Task ProcessAndSendHash(string hash)
        {
            // Process the hash and send to RabbitMQ
            await _rabbitMQService.SendMessageAsync(hash);
        }

        internal void GenerateAndSendHashes()
        {
            throw new NotImplementedException();
        }
    }
}
