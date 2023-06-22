using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace AzFunctions
{
    public class AzFunctions
    {
        [FunctionName("AzFunctions")]
        public void Run([TimerTrigger("*/60 * * * * *")] TimerInfo myTimer, ILogger log, ExecutionContext executionContext)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var config = new ConfigurationBuilder().SetBasePath(executionContext.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", true, true).AddEnvironmentVariables().Build();

            BlobContainerClient blobContainerClient = new BlobContainerClient("DefaultEndpointsProtocol=https;AccountName=straccblobaz19;AccountKey=juvesmS3YfsaJJCjZmE4215mhWig9UAfiqJlh0/8JLHn5Ixm1idOdL/aNnrFnIM12+/K1PLeVzax+AStziRQPw==;EndpointSuffix=core.windows.net", "images");

            Azure.Pageable<BlobItem> blobs = blobContainerClient.GetBlobs();
            int count = 0, deletions = 0;

            foreach(BlobItem blob in blobs)
            {
                count++;
            }

            if(count > 10)
            {
                deletions = count - 10;
            }
            //log.LogInformation(blobContainerClient.GetType().ToString());
            if(deletions > 0)
            {
                foreach (BlobItem blobItem in blobs)
                {
                    blobContainerClient.DeleteBlobIfExists(blobItem.Name);
                    log.LogInformation($"Blob Name {blobItem.Name} is deleted successfully.");
                    break;
                }
            }
            
        }
    }

}