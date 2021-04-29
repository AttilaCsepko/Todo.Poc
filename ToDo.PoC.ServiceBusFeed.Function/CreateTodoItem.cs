using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace ToDo.PoC.ServiceBusFeed.Function
{
    public static class CreateTodoItem
    {
        [FunctionName("CreateTodoItem")]
        [return: ServiceBus("todo-create-queue", EntityType.Queue)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("SendMessage function requested");

            string taskDescription = req.Query["taskDescription"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            taskDescription ??= data?.taskDescription;
            if (string.IsNullOrEmpty(taskDescription))
                return new BadRequestResult();

            log.LogInformation($"Todo Item create request has been sent with description '{taskDescription}'");
            string responseMessage = taskDescription;
            return new OkObjectResult(responseMessage);
        }
    }
}
