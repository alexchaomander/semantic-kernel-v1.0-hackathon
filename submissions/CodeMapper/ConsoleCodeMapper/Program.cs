// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

#pragma warning disable SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

using ConsoleCodeMapper;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Planning.Handlebars;

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
kernel.ImportPluginFromType<CodeMatrixPlugin>();

OpenAIPromptExecutionSettings settings = new()
{
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
};

// Start the chat session
string ask = @"
Execute the following steps:
1. Load the categories csv 'SourceCategories.csv'.
2. Show the categories values as plain text.";

ask = @"1. Load the categories csv 'SourceCategories.csv'";


HandlebarsPlanner planner = new(
    new HandlebarsPlannerConfig
    {
        MaxTokens = 3000,
        AllowLoops = true
    });
HandlebarsPlan plan = await planner.CreatePlanAsync(kernel, ask).ConfigureAwait(false);


// Print the plan to the console
Console.WriteLine($"Plan: {plan}");

// Execute the plan
var kargs = new KernelArguments();
var result = plan.InvokeAsync(kernel, new Dictionary<string, object?>(), CancellationToken.None);//.Trim();

return;

// Print the result to the console
Console.WriteLine($"Results: {result}");


while (true)
{
    // Get the user's message
    Console.Write($"User > {ask}");
    if (string.IsNullOrEmpty(ask))
    {
        ask = Console.ReadLine()!;
    }

    // Invoke the kernel
    var results = await kernel!.InvokePromptAsync(ask, new(settings)).ConfigureAwait(false);

    // Print the results
    Console.WriteLine($"Assistant > {results}");
    ask = string.Empty;
}
#pragma warning restore SKEXP0060 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
