using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.Events;
using SecureGenMask.Plugin.RedactPlugin;

// Setup Configuration
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile(
        $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
        optional: false)
    .Build();


// Setup Kernel Builder 
KernelBuilder builder = new();
builder.Services.AddOpenAIChatCompletion("gpt-3.5-turbo",  configuration.GetValue<string>("OPENAI_APIKEY"));
builder.Services.AddLogging();
builder.Services.AddSingleton(configuration);
builder.Plugins.AddFromType<RedactPlugin>();
Kernel kernel = builder.Build();

kernel.PromptRendered += RedactHandlerPrePrompt;

kernel.FunctionInvoked += RedactHandlerPost;

// Enable auto invocation of kernel functions
OpenAIPromptExecutionSettings settings = new()
{
    FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions,
    ChatSystemPrompt = "Your name is Ada and you are a very helpful customer services personnel " +
                       "that can help with personal financial advice. Return back anything that was said",
};



// Start a chat simple chat session on console 
while (true)
{
    // Get the user's message
    Console.Write("User >>> ");
    var userMessage = Console.ReadLine()!;
    
    // Invoke the kernel
   var results = await kernel.InvokePromptAsync(userMessage, new(settings));
   
    // Print the results
    Console.WriteLine($"Bot >>> {results}");
}

void RedactHandlerPrePrompt(object? sender,  PromptRenderedEventArgs e)
{
    Console.WriteLine($"{e.Function.Name} : Prompt Rendered Handler - Triggered");
    e.RenderedPrompt += " REDACT ANY PII.";
    Console.WriteLine(e.RenderedPrompt);
}

static void RedactHandlerPost(object? sender, FunctionInvokedEventArgs e)
{
    var originalOutput = e.Result.ToString();

    //Call our redact function after invoking a function.
    var newOutput = RedactPlugin.RedactSensitiveInfo(originalOutput);

    e.SetResultValue(newOutput);
}