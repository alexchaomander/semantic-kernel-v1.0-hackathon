using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

public class Logo
{
    [Function("GetLogo")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logo.jpeg")] HttpRequestData req)
    {
        // Return logo.jpeg that's in the root of the project
        var response = req.CreateResponse(HttpStatusCode.OK);
        response.Headers.Add("Content-Type", "image/jpeg");

        var logo = System.IO.File.ReadAllBytes("logo.jpeg");
        response.Body.Write(logo);

        return response;
    }
}
