using System.Text;
using RabbitMQ.Client;

namespace sender;

public class AMQPSender
{
    public static void Main(string[] args)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        
        Console.Write("number of messages to send: ");
        int numberOfMessagesToSend = int.Parse(Console.ReadLine());
        
        int numberOfMessages = numberOfMessagesToSend > 0 ? numberOfMessagesToSend : 1;
        for (int i = 0; i < numberOfMessages; i++)
        {
            long shares = ((long) ((new Random().NextDouble() * 4000) + 1));
            string text = "name for customer id " + shares;

            byte[] message = Encoding.UTF8.GetBytes(text);
            String routingKey = "trade.eq.q";
            Console.WriteLine("get " + text);
            channel.BasicPublish("", routingKey, null, message);
        }

        Console.ReadLine();
    }
}