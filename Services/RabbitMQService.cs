using EasyNetQ;
using System.Threading.Tasks;

namespace HashProcessorApp.Services
{
    public class RabbitMQService
    {
        private readonly IBus _bus;

        // Constructor to initialize EasyNetQ's IBus
        public RabbitMQService(string rabbitMqConnectionString)
        {
            _bus = RabbitHutch.CreateBus(rabbitMqConnectionString);
        }

        // Method to send a message to RabbitMQ
        public async Task SendMessageAsync(string message)
        {
            // Publish the message to the RabbitMQ exchange using EasyNetQ
            await _bus.PubSub.PublishAsync(message);  // Correct usage of EasyNetQ to publish message
        }

        // Optionally, you can subscribe to messages if needed
        public void SubscribeToMessages()
        {
            // Example of subscribing to a queue to receive messages
            _bus.PubSub.Subscribe<string>("hash_queue", message =>
            {
                // Process the incoming message here
                // In this case, the message is the hash string
                Console.WriteLine($"Received message: {message}");
            });
        }

        // Cleanup to dispose of the bus connection
        public void Dispose()
        {
            _bus?.Dispose();
        }
    }
}
