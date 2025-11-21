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

        public Task<List<string>> GetContainersAndBlobsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
