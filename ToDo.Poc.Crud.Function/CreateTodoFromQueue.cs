using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ToDo.Poc.Crud.Function
{
    public static class CreateTodoFromQueue
    {
        [FunctionName("CreateTodoFromQueue")]
        public static void Run([ServiceBusTrigger("todo-create-queue")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
