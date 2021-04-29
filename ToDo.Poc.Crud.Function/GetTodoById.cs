using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ToDo.Poc.Crud.Function.Models;

namespace ToDo.Poc.Crud.Function
{
    public static class GetTodoById
    {
        [FunctionName("GetTodoById")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Id** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("GetTodoById function triggered");

            string id = req.Query["id"];

            var collectionUri = UriFactory.CreateDocumentCollectionUri("ToDoList", "Items");
            var document = client.CreateDocumentQuery(collectionUri)
                .Where(t => t.Id == id)
                .AsEnumerable()
                .FirstOrDefault();
            if (document == null)
            {
                log.LogWarning($"Todo list item with id #{id} is not exist");
                return new NotFoundResult();
            }

            //var todo = JsonConvert.DeserializeObject<Todo>(document.ToString());
            Todo todo =(dynamic)document;

            return new OkObjectResult(todo);
        }
    }
}

