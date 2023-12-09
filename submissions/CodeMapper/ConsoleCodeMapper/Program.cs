// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.
using System.Text.Json;
using ConsoleCodeMapper;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;

// *** SETUP ***
IConfiguration configuration = new ConfigurationBuilder()
    .AddDefaultConfiguration()
    .Build();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddConfiguration(configuration);

// Builds the service collection that we will pass to SK.
var serviceProvider = builder.Services
    .AddLogging(cfg => cfg
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole())
    .AddSemanticKernel(configuration)
    .BuildServiceProvider();

// *** ACTION ***

// Creates the kernel and imports the new plugin
Kernel kernel = serviceProvider.GetRequiredService<Kernel>();

OpenAIPromptExecutionSettings settings = new()
{
    Temperature = 0.2,
    MaxTokens = 4000,
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
};

// Load the SourceCodes.csv file
string ask =
    "Get the categories referenced in the csv file {{$input}} and return them as a JSON array. " +
    "Do not add any additional text to the response.";

var getCategoriesCall = kernel.CreateFunctionFromPrompt(ask, settings);
var catResult = await kernel.InvokeAsync(getCategoriesCall, new("SourceCodes.csv")).ConfigureAwait(false);

Console.WriteLine(catResult);

//------------------------------------------------------------
var categories = JsonSerializer.Deserialize<CategoryItem[]>(catResult.ToString())!;

var getCodesCall = kernel.CreateFunctionFromPrompt(
       "Get the codes of category {{$input}} from the file SourceCodes.csv and return them as a JSON array. " +
       "Return the response without any additional text except the JSON array." +
       "Example of response:\r\n " +
       "[{ Id: 1, Description: 'bla bla'}, { Id: 2, Description: 'something else'}]" +
       "IMPORTANT: Make sure the resonse does not contain more than one array.",
    settings);

for (int i = 0; i < categories.Length; i++)
{
    CategoryItem? category = categories[i];

    var jsonCodesResult = await kernel.InvokeAsync(getCodesCall, new(category.Id))
        .ConfigureAwait(true);

    var json = jsonCodesResult.ToString();
    // trim json up to and including the last ']'
    var trimmedJson = json.Substring(0, json.IndexOf(']') + 1);

    var codes = JsonSerializer.Deserialize<CodeItem[]>(json);

    Console.WriteLine(trimmedJson);
}
