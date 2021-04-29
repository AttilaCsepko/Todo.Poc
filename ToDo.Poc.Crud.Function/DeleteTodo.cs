using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ToDo.Poc.Crud.Function
{
    public static class DeleteTodo
    {
        [FunctionName("DeleteTodo")]
        //[OpenApiOperation(operationId: "Run", tags: new[] { "name" })]
        //[OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        //[OpenApiParameter(name: "name", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **Name** parameter")]
        //[OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            [CosmosDB(ConnectionStringSetting = "CosmosDBConnection")] DocumentClient client,
            ILogger log)
        {
            log.LogInformation("Delete a todo list item");
            string id = req.Query["id"];

            Uri collectionUri = UriFactory.CreateDocumentCollectionUri("ToDoList", "Items");
            var document = client.CreateDocumentQuery(collectionUri)
                .Where(t => t.Id == id)
                .AsEnumerable()
                .FirstOrDefault();
            if (document == null)
            {
                log.LogWarning($"Todo list item with id #{id} is not exist");
                return new NotFoundResult();
            }
            await client.DeleteDocumentAsync(document.SelfLink);
            return new OkResult();
        }
    }
}

