using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        public async Task<bool> DeleteBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            return await blobClient.DeleteIfExistsAsync();
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

        public async Task<string> GetBlob(string blobName, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            return blobClient.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string blobName, IFormFile formFile, string containerName)
        {
            BlobContainerClient blobContainerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var httpHeaders = new BlobHttpHeaders
            {
                ContentType = formFile.ContentType
            };

            var result = await blobClient.UploadAsync(formFile.OpenReadStream(), httpHeaders);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
