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
    .AddLogging(cfg => cfg.AddConsole())
    .AddSemanticKernel(configuration)
    .BuildServiceProvider();

// *** ACTION ***

// Creates the kernel and imports the new plugin
Kernel kernel = serviceProvider.GetRequiredService<Kernel>();

var kargs = new KernelArguments();
OpenAIPromptExecutionSettings settings = new()
{
    MaxTokens = 4000,
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
};

// Load the SourceCodes.csv file
string ask = "Get the categories referenced in the file SourceCodes.csv and return them as a JSON array. Do not add any additional text to the response.";
Console.WriteLine(ask);
var jsonCategoriesResult = await kernel.InvokePromptAsync(ask, new(settings)).ConfigureAwait(false);

Console.WriteLine(jsonCategoriesResult);

var categories = JsonSerializer.Deserialize<CategoryItem[]>(jsonCategoriesResult.ToString())!;

for (int i = 0; i < categories.Length; i++)
{
    CategoryItem? category = categories[i];

    ask = $"Get the codes for category {category.Id} from the file SourceCodes.csv and return them as a JSON array. Do not add any additional text to the response.";
    Console.WriteLine(ask);

    var jsonCodesResult = await kernel.InvokePromptAsync(ask, new(settings)).ConfigureAwait(true);
    var codes = JsonSerializer.Deserialize<CodeItem[]>(jsonCodesResult.ToString());

    Console.WriteLine(jsonCodesResult);
}
