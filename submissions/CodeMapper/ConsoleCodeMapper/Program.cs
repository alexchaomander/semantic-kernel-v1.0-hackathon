// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using ConsoleCodeMapper;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Plugins.Core;

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
var kernel = serviceProvider.GetRequiredService<Kernel>();
kernel.ImportPluginFromType<CodeMatrixPlugin>();
#pragma warning disable SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
kernel.ImportPluginFromType<FileIOPlugin>();
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

OpenAIPromptExecutionSettings settings = new()
{
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions
};

// Start the chat session
string userMessage = "Load the spreadsheet 'CodeMatrixData.xlsx' and page 'PageOne' and then display the results.";
while (true)
{
    // Get the user's message
    Console.Write($"User > {userMessage}");
    if (string.IsNullOrEmpty(userMessage))
    {
        userMessage = Console.ReadLine()!;
    }

    // Invoke the kernel
    var results = await kernel!.InvokePromptAsync(userMessage, new(settings)).ConfigureAwait(false);

    // Print the results
    Console.WriteLine($"Assistant > {results}");
    userMessage = string.Empty;
}
