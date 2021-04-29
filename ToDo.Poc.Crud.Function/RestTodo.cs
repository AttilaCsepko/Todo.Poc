using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Poc.Crud.Function.Models;

namespace ToDo.Poc.Crud.Function
{
    public static class RestTodo
    {
        private const string Route = "todo";
        private const string DatabaseName = "ToDoList";
        private const string CollectionName = "Items";

        [FunctionName("Rest-CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            [CosmosDB(
                DatabaseName,
                CollectionName,
                ConnectionStringSetting = "CosmosDBConnection")]
            IAsyncCollector<object> todos, ILogger log)
        {
            log.LogInformation("Creating a new todo list item");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            await todos.AddAsync(new { id = todo.Id, todo.CreatedTime, todo.IsCompleted, todo.TaskDescription });
            return new OkObjectResult(todo);
        }

        [FunctionName("Rest-GetTodos")]
        public static IActionResult GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
            [CosmosDB(
                DatabaseName,
                CollectionName,
                ConnectionStringSetting = "CosmosDBConnection",
                SqlQuery = "SELECT * FROM c order by c._ts desc")]
                IEnumerable<Todo> todos,
            ILogger log)
        {
            log.LogInformation("Getting todo list items");
            return new OkObjectResult(todos);
        }

        [FunctionName("Rest-GetTodoById")]
        public static IActionResult GetTodoById(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")] HttpRequest req,
            [CosmosDB(DatabaseName, CollectionName, ConnectionStringSetting = "CosmosDBConnection",
                Id = "{id}")] Todo todo,
            ILogger log, string id)
        {
            log.LogInformation("Getting todo item by id");

            if (todo == null)
            {
                log.LogInformation($"Item {id} not found");
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }

        [FunctionName("Rest-UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")]
                DocumentClient client,
            ILogger log, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
            var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                            .AsEnumerable().FirstOrDefault();
            if (document == null)
            {
                return new NotFoundResult();
            }


            document.SetPropertyValue("IsCompleted", updated.IsCompleted);
            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                document.SetPropertyValue("TaskDescription", updated.TaskDescription);
            }

            await client.ReplaceDocumentAsync(document);

            Todo todoUpdated = (dynamic)document;

            return new OkObjectResult(todoUpdated);
        }

        [FunctionName("Rest-DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log, string id)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(DatabaseName, CollectionName);
            var document = client.CreateDocumentQuery(collectionUri).Where(t => t.Id == id)
                    .AsEnumerable().FirstOrDefault();
            if (document == null)
            {
                return new NotFoundResult();
            }
            await client.DeleteDocumentAsync(document.SelfLink);
            return new OkResult();
        }

    }
}
