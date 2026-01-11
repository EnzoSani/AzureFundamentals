using AzureLunnyFunc.Data;
using AzureLunnyFunc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureLunnyFunc;

public class GroceryAPI
{
    private readonly ILogger<GroceryAPI> _logger;
    private readonly ApplicationDbContext _dbContext;

    public GroceryAPI(ILogger<GroceryAPI> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }


    [Function("GetGrocery")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get",Route ="GroceryList")] HttpRequest req)
    {
        _logger.LogInformation("Getting Grocery List Items.");


        return new OkObjectResult(_dbContext.GroceryItems.ToList());
    }

    [Function("GetGroceryById")]
    public IActionResult GetGroceryById([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")] HttpRequest req, string id)
    {
        _logger.LogInformation("Getting Grocery List Item by ID.");
        var item = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id); // Assuming Id is a string based on earlier file view

        if (item == null)
        {
           return new NotFoundResult();
        }

        return new OkObjectResult(item);
    }

    [Function("UpdateGrocery")]
    public async Task<IActionResult> UpdateGrocery([HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/{id}")] HttpRequest req, string id)
    {
        _logger.LogInformation("Updating Grocery List Item.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        GroceryItem_Upsert? data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

        var item = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id);
        
        if (item == null)
        {
            return new NotFoundResult();
        }

        item.Name = data.Name;
        
        _dbContext.SaveChanges();

        return new OkObjectResult(item);
    }

    [Function("DeleteGrocery")]
    public IActionResult DeleteGrocery([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/{id}")] HttpRequest req, string id)
    {
        _logger.LogInformation("Deleting Grocery List Item.");

        var item = _dbContext.GroceryItems.FirstOrDefault(x => x.Id == id); // Assuming Id is a string

        if (item == null)
        {
            return new NotFoundResult();
        }

        _dbContext.GroceryItems.Remove(item);
        _dbContext.SaveChanges();

        return new OkResult();
    }

    [Function("CreateGrocery")]
    public async Task<IActionResult> CreateGrocery([HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req)
    {
        _logger.LogInformation("Creating Grocery List Item.");
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        GroceryItem_Upsert? data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);

        GroceryItem groceryItem = new GroceryItem
        {
            Name = data.Name
        };
        _dbContext.GroceryItems.Add(groceryItem);
        _dbContext.SaveChanges();

        return new OkObjectResult(groceryItem);
    }
}