using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Local.DataAccess;

namespace Azure.CopyFiles
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("Started copying files to Azure Blob Storage");
            try
            {
                CopyFilesAsync().Wait();
                Console.WriteLine("Successfully copying files to Azure Blob Storage");
                //SaveFiles();
                //Console.WriteLine("Successfully saving files to SQL Server Database");
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

        private static void SaveFiles()
        {
            var files = Directory.GetFiles(CloudConfigurationManager.GetSetting("DataFolder"));
            Array.Sort(files, StringComparer.InvariantCulture);
            using (var db = new AzureTestModelContainer())
            {
                foreach (var file in files)
                {
                    var doc = new XmlDocument();
                    doc.Load(file);
                    var employees = new List<Employee>();
                    foreach (XmlNode node in doc.DocumentElement)
                    {
                        employees.Add(new Employee { Name = node["Name"].InnerText, Location = node["Location"].InnerText });
                    }
                    db.Employees.AddRange(employees);
                    db.SaveChanges();
                }
            }
        }

        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            return CloudStorageAccount.Parse(storageConnectionString);
        }
    }
}