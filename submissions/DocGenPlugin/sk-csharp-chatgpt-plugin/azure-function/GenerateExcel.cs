using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Helpers;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace DocGenPlugin
{
    public class GenerateExcelDocument
    {
        private readonly ILogger _logger;
        ExcelWriter writer { get; set; }
        ContentService service { set; get; }
        public GenerateExcelDocument(ILoggerFactory loggerFactory, ContentService contentService, ExcelWriter writer)
        {
            this.service = contentService;
            this.writer = writer;
            _logger = loggerFactory.CreateLogger<GenerateExcelDocument>();
        }

        [OpenApiOperation(operationId: "GenerateExcelDocument", tags: new[] { "ExecuteFunction" }, Description = "Generate Excel document from instruction.")]
        [OpenApiParameter(name: "instruction", Description = "instruction to generate content", Required = true, In = ParameterLocation.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/octet-stream", bodyType: typeof(byte[]), Description = "Return the Excel document.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
        [Function("GenerateExcelDocument")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var instruction = req.Query["instruction"].ToString();

            if (!string.IsNullOrEmpty(instruction))
            {
                var json = await service.GenerateContent(instruction);
                var contents = JsonConvert.DeserializeObject<List<ContentObject>>(json);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/octet-stream");

                var bytes = writer.WriteToXlsx(contents);
                //var base64 = $"data:application/octet-stream;base64,{Convert.ToBase64String(bytes)}";
                //response.WriteString(base64);
                response.WriteBytes(bytes);
                _logger.LogInformation($"write excel success: {bytes.Length} bytes");

                return response;
            }
            else
            {
                HttpResponseData response = req.CreateResponse(HttpStatusCode.BadRequest);
                response.Headers.Add("Content-Type", "application/json");
                response.WriteString("Please pass instruction in the request query");

                return response;
            }
        }
    }
}