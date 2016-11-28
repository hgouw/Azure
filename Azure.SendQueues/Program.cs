using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Azure.SendQueues
{
    class Program
    {
        static void Main()
        {
            var storageAccount = new CloudStorageAccount(new StorageCredentials(CloudConfigurationManager.GetSetting("AccountName"), CloudConfigurationManager.GetSetting("AccountKeyValue")), true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("optus");
            var blobs = container.ListBlobs();
            foreach (var blob in blobs)
            {
                if (blob is CloudBlockBlob)
                {
                    var blobName = ((CloudBlockBlob)blob).Name;
                    output.Add(blobName);
                }
            }
        }
    }
}
