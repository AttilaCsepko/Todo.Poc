using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Poc.Crud.Function.Models;

namespace ToDo.Poc.Crud.Function
{
    public static class DeleteTodoFromQueue
    {
        [FunctionName("DeleteTodoFromQueue")]
        public static async Task Run([ServiceBusTrigger("todo-delete-queue")] string myQueueItem,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {

            log.LogInformation("Delete a todo list item from ServiceBus");
            log.LogInformation($"ServiceBus message: {myQueueItem}");

            var id = JsonConvert.DeserializeObject<ServiceBusMessage>(myQueueItem).Value;

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("ToDoList", "Items");
            var document = client.CreateDocumentQuery(collectionUri)
                .Where(t => t.Id == id)
                .AsEnumerable()
                .FirstOrDefault();
            if (document == null)
            {
                log.LogWarning($"Todo list item with id #{id} is not exist");
                return;
            }
            await client.DeleteDocumentAsync(document.SelfLink);

        }
    }
}
