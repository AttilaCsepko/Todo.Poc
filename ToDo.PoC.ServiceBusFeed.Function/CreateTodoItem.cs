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
            //string body = string.Empty;
            //using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            //{
            //    body = await reader.ReadToEndAsync();
            //    log.LogInformation($"Message body : {body}");
            //}
            //log.LogInformation($"SendMessage processed.");
            //return body;


            string todoTitle = req.Query["title"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            todoTitle ??= data?.title;
            if (string.IsNullOrEmpty(todoTitle))
                return new BadRequestResult();
            
            string responseMessage = todoTitle;
            //TODO: format json and add category

            return new OkObjectResult(responseMessage);
        }
    }
}
