using System;
using Azure.Storage.Queues.Models;
using AzureLunnyFunc.Data;
using AzureLunnyFunc.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureLunnyFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;
        private readonly ApplicationDbContext _dbContext;

        public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function(nameof(OnQueueTriggerUpdateDatabase))]
        public void Run([QueueTrigger("SalesRequestInBound")] QueueMessage message)
        {
            string messageBody = message.Body.ToString();
            SalesRequest? salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);

            if (salesRequest != null)
            {
                 salesRequest.Status = ""; 
                _dbContext.SalesRequests.Add(salesRequest);
                _dbContext.SaveChanges();
                _logger.LogInformation($"SalesRequest with ID: {salesRequest.Id} has been added to the database.");
            }
            else
            {
                _logger.LogError("Failed to deserialize the sales request from the queue message.");
            }
        }
    }
}
