using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using ToDo.Poc.Crud.Function.Models;

namespace ToDo.Poc.Crud.Function
{
    public static class CreateTodoFromQueue
    {
        [FunctionName("CreateTodoFromQueue")]
        public static async Task Run([ServiceBusTrigger("todo-create-queue")]string myQueueItem,
            [CosmosDB(databaseName: "ToDoList", collectionName: "Items", ConnectionStringSetting = "CosmosDBConnection")] IAsyncCollector<object> todos,
            ILogger log)
        {
            log.LogInformation("Creating a new todo list item from ServiceBus");
            log.LogInformation($"ServiceBus message: {myQueueItem}");

            var input = JsonConvert.DeserializeObject<ServiceBusMessage>(myQueueItem);

            var todo = new Todo() { TaskDescription = input.Value };
            await todos.AddAsync(new { id = todo.Id, todo.CreatedTime, todo.IsCompleted, todo.TaskDescription });
        }
    }
}
