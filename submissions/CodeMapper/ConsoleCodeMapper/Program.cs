// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.
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

var ask = @"You are a code mapper that maps category codes after they are loaded from a CSV file.

Do the following:
1. Load categories from fileName 'SourceCategories.csv'.
2. Load categories from fileName 'DestinationCategories.csv'.
3. Create a mapping between the category codes of both lists by matching the field 'Description' on similar meanings.

Examples:
-Given Source Description 'Cancer', we would find matches with Destination Description 'Malignant Neoplasm', 'Malignant Tumor', etc.
-Given Source Description 'Sex Codes', we would find matches with Destination Description 'Gender', 'Sessualita'', etc.
-Given Source Description 'Drugs', we would find matches with Destination Description 'Narcotics', 'Illegal drugs'', etc.

4. The result would display a list of mappings between the source and destination categories.
Examples:
-Source Category: 'Cancer' -> Destination Category: 'Malignant Neoplasm'
-Source Category: 'Drugs' -> Destination Category: 'Illegal drugs' 
";


var kargs = new KernelArguments();
OpenAIPromptExecutionSettings settings = new()
{
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
};

while (true)
{
    // Get the user's message
    Console.Write($"User > {ask}");
    if (string.IsNullOrEmpty(ask))
    {
        ask = Console.ReadLine()!;
    }

    if (string.IsNullOrEmpty(ask))
        continue;

    // Invoke the kernel
    var results = await kernel!.InvokePromptAsync(ask, new(settings)).ConfigureAwait(false);

    // Print the results
    Console.WriteLine($"Assistant > {results}");
    ask = string.Empty;
}
