using AzureLunnyFunc.Data;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.Configuration.AddUserSecrets<Program>();

builder.ConfigureFunctionsWebApplication();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

string connectionString = builder.Configuration["AzureDatabase"];

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'AzureDatabase' is not found.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<ApplicationDbContext>(opt =>
//opt.UseSqlServer(builder.Configuration.GetConnectionString("AzureDatabase")));

builder.Build().Run();
