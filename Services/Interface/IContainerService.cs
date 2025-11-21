namespace AzureBlobProject.Services.Interface
{
    public interface IContainerService
    {
        Task<List<string>> GetContainersAndBlobsAsync();
        Task<List<string>> GetAllContainerAsync();
        Task CreateContainerAsync(string containerName);
        Task DeleteContainerAsync(string containerName);
    }
}
