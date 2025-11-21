using Azure.Storage.Blobs;
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
        public Task CreateContainerAsync(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContainerAsync(string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetAllContainerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetContainersAndBlobsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
