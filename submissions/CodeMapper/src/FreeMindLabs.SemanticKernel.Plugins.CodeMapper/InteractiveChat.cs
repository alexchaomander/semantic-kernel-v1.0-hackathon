// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.PromptTemplate.Handlebars;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <summary>
/// This is the main application service.
/// This takes console input, then sends it to the configured AI service, and then prints the response.
/// All conversation history is maintained in the chat history.
/// </summary>
public class InteractiveChat : IHostedService
{
    private readonly Kernel _kernel;
    private readonly IChatEvents _eventHandler;
    private readonly IHostApplicationLifetime _lifeTime;
    private KernelFunction _prompt;

    public InteractiveChat(Kernel kernel, IChatEvents eventHandler, IHostApplicationLifetime lifeTime)
    {
        // Load prompt from resource        
        var yaml = File.ReadAllText($"prompts/{this.GetType().Name}.yaml");
        this._prompt = kernel.CreateFunctionFromPromptYaml(
            text: yaml,
            promptTemplateFactory: new HandlebarsPromptTemplateFactory()
        );

        this._kernel = kernel;
        this._eventHandler = eventHandler;
        this._lifeTime = lifeTime;
    }

    /// <summary>
    /// Start the service.
    /// </summary>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(() => this.ExecuteAsync(cancellationToken), cancellationToken);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Stop a running service.
    /// </summary>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <summary>
    /// The main execution loop. It will use any of the available plugins to perform actions
    /// </summary>
    private async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        ChatHistory chatMessages = [];
        IChatCompletionService chatCompletionService = this._kernel.GetRequiredService<IChatCompletionService>();

        // Loop till we are cancelled
        var initialPromptSent = false;

        while (!cancellationToken.IsCancellationRequested)
        {
            string ask;
            if (!initialPromptSent)
            {
                initialPromptSent = true;
                ask = await this._eventHandler.GetInitialPromptAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                ask = await this._eventHandler.GetAskAsync(cancellationToken).ConfigureAwait(false);
            }

            chatMessages.AddUserMessage(ask);

            // Get the chat completions
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions,
                //MaxTokens = 4000,
            };
            //IAsyncEnumerable<StreamingChatMessageContent> result =
            //    chatCompletionService.GetStreamingChatMessageContentsAsync(
            //        chatMessages,
            //        executionSettings: openAIPromptExecutionSettings,
            //        kernel: this._kernel,
            //        cancellationToken: cancellationToken);

            IAsyncEnumerable<StreamingChatMessageContent> result =
                this._kernel.InvokeStreamingAsync<StreamingChatMessageContent>(
                    this._prompt,
                    arguments: new(openAIPromptExecutionSettings) {
                        { "messages", chatMessages }
                    },
                    cancellationToken);

            // Print the chat completions
            ChatMessageContent? chatMessageContent = null;

            await this._eventHandler.BeginResponseAsync(cancellationToken).ConfigureAwait(false);

            await foreach (var content in result)
            {
                if (content.Role.HasValue)
                {
                    chatMessageContent = new(
                        content.Role ?? AuthorRole.Assistant,
                        content.ModelId!,
                        content.Content!,
                        content.InnerContent,
                        content.Encoding,
                        content.Metadata
                    );
                }
                //System.Console.Write(content.Content);
                chatMessageContent!.Content += content.Content;

                await this._eventHandler.AppendResponseAsync(content.Content, cancellationToken)
                    .ConfigureAwait(false);

            }
            //System.Console.WriteLine();
            await this._eventHandler.EndResponseAsync(cancellationToken).ConfigureAwait(false);

            chatMessages.AddMessage(chatMessageContent!);
        }
    }
}
