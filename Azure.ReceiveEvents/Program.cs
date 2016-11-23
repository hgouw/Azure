using System;
using System.Text;
using Microsoft.ServiceBus.Messaging;

namespace Azure.ReceiveEvents
{
    class Program
    {
        static void Main()
        {
            try
            {
                var factory = MessagingFactory.CreateFromConnectionString("Endpoint=sb://optus-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XmnjtQXdULTRKNm0sv8rNHCjTH+j0Iejx8p1gntJP74=" + ";TransportType=Amqp");
                var client = factory.CreateEventHubClient("optus");
                var group = client.GetDefaultConsumerGroup();
                var receiver = group.CreateReceiver(client.GetRuntimeInformation().PartitionIds[0]);
                bool receive = true;
                while (receive)
                {
                    var message = receiver.Receive();
                    Console.WriteLine("Received message: " + Encoding.UTF8.GetString(message.GetBytes()));
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}