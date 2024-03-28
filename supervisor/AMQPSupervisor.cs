using RabbitMQ.Client;

namespace supervisor;
public class AMQPSupervisor
{
    private List<AMQPConsumer> consumers = new();
    private IConnection connection;
    
    public static void Main(string[] args)
    {
        var app = new AMQPSupervisor();
        Console.Write("Keep Alive (ms): ");
        long keepAlive = long.Parse(Console.ReadLine());
        Console.Write("Enable Supervisor (y/n): ");
        bool isSupervisor = Console.ReadLine().Equals("y", StringComparison.OrdinalIgnoreCase);
        Console.Write("Initial Consumers: ");
        long consumerCount = long.Parse(Console.ReadLine());
        
        app.Run(keepAlive, isSupervisor, consumerCount);
    }

    public void Run(long keepAlive, bool isSupervisor, long consumerCount)
    {
        Console.WriteLine("Starting service");
        if (isSupervisor)
        {
            Console.WriteLine("Starting supervisor");
        }

        var factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "trade.eq.q", durable: false, exclusive: false, autoDelete: false, arguments: null);

        for (int i = 0; i < consumerCount; i++)
        {
            StartConsumer();
        }

        if (isSupervisor)
        {
            long check = 1000;
            long checkCounter = 0;
            while (true)
            {
                try
                {
                    if (channel.IsOpen)
                    {
                        long queueDepth = channel.MessageCount("trade.eq.q");
                        long consumersNeeded = queueDepth / 2;
                        long diff = Math.Abs(consumersNeeded - consumers.Count);

                        if (consumersNeeded > consumers.Count)
                        {
                            for (int i = 0; i < diff; i++)
                            {
                                StartConsumer();
                            }
                        }
                        else if (checkCounter >= keepAlive)
                        {
                            checkCounter = 0;
                            for (int i = 0; i < diff; i++)
                            {
                                StopConsumer(consumerCount);
                            }
                        }

                        checkCounter += check;
                        Thread.Sleep((int)check);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }
               
            }
        }
    }

    private void StartConsumer()
    {
        Console.WriteLine("Starting consumer...");
        var consumer = new AMQPConsumer();
        consumers.Add(consumer);
        new Thread(() => consumer.Start(connection)).Start();
    }

    private void StopConsumer(long consumerCount)
    {
        if (consumers.Count > consumerCount)
        {
            Console.WriteLine("Removing consumer...");
            var consumer = consumers[0];
            consumer.Shutdown(); 
            consumers.RemoveAt(0);
        }
    }

    
}
