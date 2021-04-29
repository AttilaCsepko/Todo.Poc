using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Poc.Crud.Function.Models;

namespace ToDo.Poc.Crud.Function
{
    public static class UpdateTodo
    {
        [FunctionName("UpdateTodo")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("Update Todo item triggered");
            string id = req.Query["id"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            var collectionUri = UriFactory.CreateDocumentCollectionUri("ToDoList", "Items");
            var document = client.CreateDocumentQuery(collectionUri)
                .Where(t => t.Id == id)
                .AsEnumerable()
                .FirstOrDefault();

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

            Todo todo2 = (dynamic)document;

            return new OkObjectResult(todo2);
        }
    }
}

