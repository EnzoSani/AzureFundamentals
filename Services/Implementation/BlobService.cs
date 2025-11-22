using Azure.Storage.Blobs;
using AzureBlobProject.Services.Interface;
using NuGet.Protocol;

namespace AzureBlobProject.Services.Implementation
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobClient)
        {
            _blobClient = blobClient;
        }
        public Task<bool> DeleteBlob(string blobName, string containerName)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync();
            var blobString = new List<string>();
            await foreach(var item in blobs)
            {
                blobString.Add(item.Name);
            }
            return blobString;
        }

        public Task<string> GetBlob(string blobName, string containerName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UploadBlob(string blobName, IFormFile formFile, string containerName)
        {
            throw new NotImplementedException();
        }
    }
}
