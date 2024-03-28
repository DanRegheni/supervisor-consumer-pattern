using RabbitMQ.Client;

namespace monitor;

public class AMQPMonitor
{
    public static void Main(string[] args)
    {
        try
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
        
            while (true) {
                long consumers = channel.ConsumerCount("trade.eq.q");
                long queueDepth = channel.MessageCount("trade.eq.q");
                Console.WriteLine("consumers: " + consumers + ", pending msgs:" + queueDepth);
                Thread.Sleep(1000);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}