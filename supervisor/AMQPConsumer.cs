using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace supervisor;

public class AMQPConsumer
{
    private volatile bool active;

    public void Start(IConnection connection)
    {
        try
        {
            active = true;
            var channel = connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicQos(0, 1, false);
            channel.BasicConsume(queue: "trade.eq.q", autoAck: false, consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                if (active)
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("getting " + message);
                    Thread.Sleep(1000); // Simulate work

                    // Acknowledge the message
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            while (active)
            {
                // Keep the consumer alive
                Thread.Sleep(10); // Sleep to prevent a tight loop
            }

            channel.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
    }

    public void Shutdown()
    {
        lock (this)
        {
            active = false;
        }
    }
}
