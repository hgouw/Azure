using System;
using System.Text;
using System.Threading;
using Microsoft.ServiceBus.Messaging;

namespace Azure.PublishEvents
{
    class Program
    {
        static void Main()
        {
            try
            {
                var factory = MessagingFactory.CreateFromConnectionString("Endpoint=sb://optus-test.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XmnjtQXdULTRKNm0sv8rNHCjTH+j0Iejx8p1gntJP74=" + ";TransportType=Amqp");
                var client = factory.CreateEventHubClient("optus");
                for (int i = 0; i < 6; i++)
                {
                    var message = Guid.NewGuid().ToString();
                    client.Send(new EventData(Encoding.UTF8.GetBytes(message)));
                    Console.WriteLine("Sent message: " + message);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}