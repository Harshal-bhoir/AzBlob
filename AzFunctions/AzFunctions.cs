using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
            int count = 0;

            foreach(BlobItem blob in blobs)
            {
                count++;
            }

            DateTimeOffset dateFuture = new DateTimeOffset(2100, 12, 12, 8, 20, 10, new TimeSpan(-5, 0, 0));
            BlobItem blobToBeDeleted = null;
            foreach (BlobItem blobItem in blobs)
            {
                DateTimeOffset dateNow = (DateTimeOffset)blobItem.Properties.LastModified;
                var newTime = DateTimeOffset.Compare(dateNow, dateFuture);
                if(newTime < 0)
                {
                    blobToBeDeleted = blobItem;
                    dateFuture = dateNow;
                }
            }

            if(count > 10)
            {
                blobContainerClient.DeleteBlobIfExists(blobToBeDeleted.Name);
                log.LogInformation($"Blob Name {blobToBeDeleted.Name} is deleted successfully.");
            }
        }
    }

}