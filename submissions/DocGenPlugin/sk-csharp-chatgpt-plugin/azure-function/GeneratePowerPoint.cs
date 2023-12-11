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
    public class GeneratePowerPoint
    {
        private readonly ILogger _logger;
        PowerPointWriter writer { get; set; }
        ContentService service { set; get; }
        public GeneratePowerPoint(ILoggerFactory loggerFactory, ContentService contentService, PowerPointWriter writer)
        {
            this.service = contentService;
            this.writer = writer;
            _logger = loggerFactory.CreateLogger<GeneratePowerPoint>();
        }

        [OpenApiOperation(operationId: "GeneratePowerPoint", tags: new[] { "ExecuteFunction" }, Description = "Generate power point document from instruction.")]
        [OpenApiParameter(name: "instruction", Description = "instruction to generate content", Required = true, In = ParameterLocation.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/octet-stream", bodyType: typeof(byte[]), Description = "Return the power point document.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: "application/json", bodyType: typeof(string), Description = "Returns the error of the input.")]
        [Function("GeneratePowerPoint")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            var instruction = req.Query["instruction"].ToString();

            if (!string.IsNullOrEmpty(instruction))
            {
                var json = await service.GenerateContent(instruction);
                var contents = JsonConvert.DeserializeObject<List<ContentObject>>(json);

                HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
                response.Headers.Add("Content-Type", "application/octet-stream");

                var bytes = writer.WriteToPptx(contents);
                //var base64 = $"data:application/octet-stream;base64,{Convert.ToBase64String(bytes)}";
                //response.WriteString(base64);
                response.WriteBytes(bytes);
                _logger.LogInformation($"write pptx success: {bytes.Length} bytes");

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