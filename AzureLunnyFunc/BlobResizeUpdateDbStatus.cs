using AzureLunnyFunc.Data;
using AzureLunnyFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace AzureLunnyFunc;

public class BlobResizeUpdateDbStatus
{
    private readonly ILogger<BlobResizeUpdateDbStatus> _logger;
    private ApplicationDbContext _dbContext;

    public BlobResizeUpdateDbStatus(ILogger<BlobResizeUpdateDbStatus> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(BlobResizeUpdateDbStatus))]
    public async Task Run([BlobTrigger("functionsalesrep-final/{name}")] Byte[] myBlobByte, string name)
    {
        var fileName = Path.GetFileNameWithoutExtension(name);
        SalesRequest? salesRequest = await _dbContext.SalesRequests.FirstOrDefaultAsync(u => u.Id == fileName);
        if (salesRequest != null)
        {
            salesRequest.Status = "Completed";
            await _dbContext.SaveChangesAsync();
        }

        _logger.LogInformation($"BlobResize Update DB Status has been completed");
    }
}