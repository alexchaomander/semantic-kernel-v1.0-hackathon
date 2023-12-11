// Copyright (c) Microsoft. All rights reserved.

using AIPlugins.AzureFunctions.Extensions;
using Helpers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Models;

const string DefaultSemanticFunctionsFolder = "Prompts";
string semanticFunctionsFolder = Environment.GetEnvironmentVariable("SEMANTIC_SKILLS_FOLDER") ?? DefaultSemanticFunctionsFolder;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddScoped<Kernel>(sp =>
        {
            KernelBuilder builder = new();
            builder.Services.AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Information));
            /*
            builder.Services.ConfigureHttpClientDefaults(c =>
            {
                // Use a standard resiliency policy
                c.AddStandardResilienceHandler().Configure(o =>
                {
                    o.Retry.ShouldHandle = args => ValueTask.FromResult(args.Outcome.Result?.StatusCode is HttpStatusCode.Unauthorized);
                });
            });*/
            // Register your AI Providers...
            var appSettings = AppSettings.LoadSettings();
            builder.WithChatCompletionService(appSettings.Kernel);
            //builder.Plugins.AddFromType<LightPlugin>();
            return builder.Build();
        })
        /*
            .AddScoped<IKernel>((providers) =>
            {
                // This will be called each time a new Kernel is needed

                // Get a logger instance
                ILogger<IKernel> logger = providers
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger<IKernel>();

                // Register your AI Providers...
                var appSettings = AppSettings.LoadSettings();
               
                var kernel = new KernelBuilder()
                    .WithChatCompletionService(appSettings.Kernel)
                    .Services.AddLogging(logger)
                    .Build();

                // Load your semantic functions...
                kernel.ImportPromptsFromDirectory(appSettings.AIPlugin.NameForModel, semanticFunctionsFolder);

                return kernel;
            })*/
            .AddScoped<IAIPluginRunner, AIPluginRunner>()
            .AddTransient<WordWriter>()
            .AddTransient<PowerPointWriter>()
            .AddTransient<ExcelWriter>()
            .AddTransient<ContentService>();
    })
    .Build();

host.Run();
