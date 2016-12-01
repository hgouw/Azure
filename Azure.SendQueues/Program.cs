using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Azure.SendQueues
{
    class Program
    {
        public static int i { get; set; }

        static void Main()
        {
            Console.WriteLine("Started sending messages to Azure Queue Storage");
            var storageAccount = new CloudStorageAccount(new StorageCredentials("hgouw", "lXr8WD/7ULcm9AIrEW9s0q2jdQaVYcTB9rSaaQZJ51m3VxZya+t9yfEpavg5OFuni/A6kYx9bLCdimNRq7roHg=="), true);
            //var storageAccount = new CloudStorageAccount(new StorageCredentials(CloudConfigurationManager.GetSetting("AccountName"), CloudConfigurationManager.GetSetting("AccountKeyValue")), true);

            // Get the Blob Storage
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("optus");
            var blobs = container.ListBlobs();

            // Get the Queue Storage
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("optus");
            queue.CreateIfNotExists();

            // For each blob send a message in the queue
            foreach (var blob in blobs)
            {
                if (blob is CloudBlockBlob)
                {
                    queue.AddMessage(new CloudQueueMessage(((CloudBlockBlob)blob).Name));
                    i++;
                }
            }
            Console.WriteLine($"Successfully sent {i} messages to Azure Queue Storage");
            Console.ReadLine();
        }
    }
}