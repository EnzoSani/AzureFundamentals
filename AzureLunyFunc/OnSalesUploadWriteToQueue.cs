
using AzureLunyFunc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureLunyFunc
{
    public class OnSalesUploadWriteToQueue
    {
        [FunctionName("OnSalesUploadWriteToQueue")]
        public async Task<IActionResult> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
                [Queue("SalesRequestInBound", Connection = "AzureWebJobsStorage")] IAsyncCollector<SalesRequest> salesRequestQueue,
                ILogger log)
        {
            log.LogInformation("Sales Request received by OnSalesUploadWriteToQueue function");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // 1. Deserializar con manejo de nulos.
            SalesRequest? data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);

            // 2. **Verificar si la deserialización fue exitosa (el punto clave).**
            if (data == null)
            {
                // Devolver un error si el cuerpo de la solicitud no era un objeto SalesRequest válido.
                return new BadRequestObjectResult("Por favor, pase un objeto SalesRequest válido en el cuerpo de la solicitud.");
            }

            // 3. Si no es nulo, se agrega a la cola sin advertencia.
            await salesRequestQueue.AddAsync(data);

            string responseMessage = $"Sales Request has been received for {data.Name}.";
            return new OkObjectResult(responseMessage);
        }
    }
}
