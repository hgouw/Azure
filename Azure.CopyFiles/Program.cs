namespace Azure.CopyFilesSample
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Started copying files to Azure Blob Storage");
            try
            {
                CopyFilesAsync().Wait();
                Console.WriteLine("Successfully copied files to Azure Blob Storage");
            }
            catch (FormatException)
            {
                Console.WriteLine("Please confirm the AccountName and AccountKey are valid in the app.config file");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Please confirm the AccountName and AccountKey are valid in the app.config file");
            }
            catch (StorageException)
            {
                Console.WriteLine("Please make sure you have started the Azure Storage Emulator");
            }
            Console.ReadLine();
        }

        private static async Task CopyFilesAsync()
        {
            var storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageAccountConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();

            var blobContainer = blobClient.GetContainerReference(CloudConfigurationManager.GetSetting("ContainerName"));
            await blobContainer.CreateIfNotExistsAsync();

            var files = Directory.GetFiles(CloudConfigurationManager.GetSetting("DataFolder"));
            Array.Sort(files, StringComparer.InvariantCulture);
            foreach (var file in files)
            {
                var blockBlob = blobContainer.GetBlockBlobReference(Path.GetFileName(file));
                await blockBlob.UploadFromFileAsync(file, FileMode.Open);
            }
        }

        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            return CloudStorageAccount.Parse(storageConnectionString);
        }
    }
}