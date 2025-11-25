using AzureBlobProject.Models;
using System.Reflection.Metadata;

namespace AzureBlobProject.Services.Interface
{
    public interface IBlobService
    {
        Task<string> GetBlob(string blobName,string containerName);
        Task<List<string>> GetAllBlobs(string containerName);
        Task<bool> UploadBlob(string blobName, IFormFile formFile,string containerName, BlobModel blob);
        Task<bool> DeleteBlob(string blobName,string containerName);
    }
}
