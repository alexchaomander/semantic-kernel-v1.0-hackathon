// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.Reflection;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;


// Depenedency injection
var builder = Host
    .CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddUserSecrets(Assembly.GetExecutingAssembly()) // Same secrets as SK and KM :smile:
            .AddEnvironmentVariables();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Warning);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<InteractiveChat>();
        services.AddSingleton<IChatEvents, ConsoleEvents>();

        var apiKey = context.Configuration["OpenAI:ApiKey"]!;
        var chatModelId = context.Configuration["OpenAI:ChatModelId"]!;

        var kbuilder = KernelExtensions
            // By using AddKernel we instantiate a KernelBuilder that uses the app's service collection.
            // Note: Pretty much any other way to do things results in two separare service collections and service providers
            // that need the same configuration. See notes for some pseudo-code.
            .AddKernel(services)
            .AddOpenAIChatCompletion(modelId: chatModelId, apiKey: apiKey)
            .Plugins.AddFromType<CodeMatrixPlugin>();
    });

// Build and run the host. This keeps the app running using the HostedService.
var host = builder.Build();
await host.RunAsync()
    .ConfigureAwait(false);
