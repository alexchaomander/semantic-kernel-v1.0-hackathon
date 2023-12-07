// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.
// testing PRs...
using System.Reflection;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;

// *** SETUP ***

IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly()) // Same secrets as SK and KM :smile:
            .Build();

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddConfiguration(configuration);

// Builds the service collection that we will pass to SK.
IServiceCollection services = builder.Services
    .AddLogging(cfg => cfg.AddConsole())
    .AddSingleton<TestDI>();

// By using AddKernel we instantiate a KernelBuilder that uses the app's service collection.
// Note: Pretty much any other way to do things results in two separare service collections and service providers
// that need the same configuration. See notes for some pseudo-code.
var kbuilder = KernelExtensions.AddKernel(services);
kbuilder.AddOpenAIChatCompletion("gpt-3.5-turbo", configuration["OpenAI:ApiKey"]!);

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// *** ACTION ***

// Creates the kernel and imports the new plugin
var kernel = serviceProvider.GetRequiredService<Kernel>();
kernel.ImportPluginFromType<CodeMatrixPlugin>();

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
