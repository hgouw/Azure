using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Threading.Tasks;

namespace Azure.RemoveFiles
{
    class Program
    {
        public static int i { get; set; }

        static void Main()
        {
            Console.WriteLine("Started removing files from Azure Blob Storage");
            try
            {
                RemoveFilesAsync().Wait();
                Console.WriteLine($"Successfully removed {i} files from Azure Blob Storage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception at the {i}th file {ex.InnerException}");
            }
            Console.ReadLine();
        }

        private static async Task RemoveFilesAsync()
        {
            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=hgouw;AccountKey=lXr8WD/7ULcm9AIrEW9s0q2jdQaVYcTB9rSaaQZJ51m3VxZya+t9yfEpavg5OFuni/A6kYx9bLCdimNRq7roHg==");
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("optus");
            var blobs = container.ListBlobs();
            foreach (var blob in blobs)
            {
                if (blob is CloudBlockBlob)
                {
                    var block = container.GetBlockBlobReference(((CloudBlockBlob)blob).Name);
                    await block.DeleteAsync();
                    i++;
                }
            }
        }
    }
}