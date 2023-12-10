// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.Reflection;
using FreeMindLabs.SemanticKernel.Plugins.CodeMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace ConsoleCodeMapper;

/// <summary>
/// Miscellaneous extension methods for dependency injection.
/// </summary>
public static class DependencyInjectionExtensions
{
    ///// <summary>
    ///// Loads the configuration of Code Mapper from appSettings, user secrets and environment variables.
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <returns></returns>
    public static IConfigurationBuilder AddDefaultConfiguration(this IConfigurationBuilder builder)
        => new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddUserSecrets(Assembly.GetExecutingAssembly()) // Same secrets as SK and KM :smile:
                    .AddEnvironmentVariables();

    /// <summary>
    /// Adds Semantic Kernel to the service collection.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddSemanticKernel(this IServiceCollection services, IConfiguration configuration)
    {
        // By using AddKernel we instantiate a KernelBuilder that uses the app's service collection.
        // Note: Pretty much any other way to do things results in two separare service collections and service providers
        // that need the same configuration. See notes for some pseudo-code.

        var kbuilder = KernelExtensions.AddKernel(services);
        //kbuilder.AddOpenAIChatCompletion("gpt-3.5-turbo", configuration["OpenAI:ApiKey"]!);

        kbuilder.AddOpenAIChatCompletion("gpt-3.5-turbo-1106", configuration["OpenAI:ApiKey"]!);
        //kbuilder.AddOpenAIChatCompletion("gpt-4-1106-preview", configuration["OpenAI:ApiKey"]!);
        kbuilder.Plugins.AddFromType<CodeMatrixPlugin>();
        return services;
    }
}
