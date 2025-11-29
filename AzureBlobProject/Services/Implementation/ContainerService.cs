using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Services.Interface;

namespace AzureBlobProject.Services.Implementation
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient;
        public ContainerService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public async Task CreateContainerAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainerAsync(string containerName)
        {
            BlobContainerClient containerClient = _blobClient.GetBlobContainerClient(containerName);
            await containerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainerAsync()
        {
            List<string> containerName = new();
            await foreach (BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerName.Add(blobContainerItem.Name);
            }
            return containerName;
        }

        public async Task<List<string>> GetContainersAndBlobsAsync()
        {
            List<string> containerAndBlobNames = new();
            containerAndBlobNames.Add("Acconunt Name : "+ _blobClient.AccountName);
            containerAndBlobNames.Add("--------------------------------------------------");
            await  foreach(BlobContainerItem blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerAndBlobNames.Add("--" + blobContainerItem.Name);
                BlobContainerClient _blobContainer =
                    _blobClient.GetBlobContainerClient(blobContainerItem.Name);
                await foreach(BlobItem blobItem in _blobContainer.GetBlobsAsync())
                {
                    //Get metadata
                    var bobClient = _blobContainer.GetBlobClient(blobItem.Name);
                    BlobProperties blobProperties = await bobClient.GetPropertiesAsync();
                    string tempBlobAdd = blobItem.Name;
                    if (blobProperties.Metadata.ContainsKey("title"))
                    {
                        tempBlobAdd += " (" + blobProperties.Metadata["title"] + ")";
                    }
                    containerAndBlobNames.Add("------" + tempBlobAdd);
                }
                containerAndBlobNames.Add("--------------------------------------------------");
            }
            return containerAndBlobNames;
        }
    }
}
