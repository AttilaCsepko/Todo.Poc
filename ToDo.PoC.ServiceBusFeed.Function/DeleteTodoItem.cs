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
    public static class DeleteTodoItem
    {
        [FunctionName("DeleteTodoItem")]
        [return: ServiceBus("todo-delete-queue", EntityType.Queue)]
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


            string id = req.Query["id"];

            log.LogInformation($"Todo Item delete request has been sent with id '{id}'");

            string responseMessage = id;
            return new OkObjectResult(responseMessage);
        }
    }
}
