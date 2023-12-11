// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using Microsoft.Extensions.Hosting;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.AI.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Microsoft.SemanticKernel.PromptTemplate.Handlebars;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <summary>
/// This is the main application service.
/// This takes input from the user using <see cref="IChatEvents" /> and sends it to the OpenAI chat model.
/// All conversation history is maintained in the chat history.
/// </summary>
/// <remarks>
/// Heavily inspired by [Semantic Kernel Starters](https://github.com/microsoft/semantic-kernel-starters/tree/main)
/// </remarks>
public class InteractiveChat : IHostedService
{
    private readonly Kernel _kernel;
    private readonly IChatEvents _eventHandler;
    private readonly IHostApplicationLifetime _lifeTime;
    private KernelFunction _prompt;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveChat"/> class.
    /// </summary>
    /// <param name="kernel"></param>
    /// <param name="eventHandler"></param>
    /// <param name="lifeTime"></param>
    public InteractiveChat(Kernel kernel, IChatEvents eventHandler, IHostApplicationLifetime lifeTime)
    {
        // TODO: Is my yaml file any good?

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
            // Gets the user input using the event handler
            string ask;
            if (!initialPromptSent)
            {
                initialPromptSent = true;
                // TODO: This should be using Mediatr
                ask = await this._eventHandler.GetInitialPromptAsync(cancellationToken).ConfigureAwait(false);
            }
            else
            {
                // TODO: This should be using Mediatr
                ask = await this._eventHandler.GetAskAsync(cancellationToken).ConfigureAwait(false);
            }

            chatMessages.AddUserMessage(ask);

            // Get the chat completions
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                FunctionCallBehavior = FunctionCallBehavior.AutoInvokeKernelFunctions,
                //ChatSystemPrompt
                //MaxTokens = 4000,
                //Temperature
                //TopP
            };
            #region Alternative way
            // This is how it was originally done in the SK starter
            //IAsyncEnumerable<StreamingChatMessageContent> result =
            //    chatCompletionService.GetStreamingChatMessageContentsAsync(
            //        chatMessages,
            //        executionSettings: openAIPromptExecutionSettings,
            //        kernel: this._kernel,
            //        cancellationToken: cancellationToken);
            #endregion

            IAsyncEnumerable<StreamingChatMessageContent> result =
                this._kernel.InvokeStreamingAsync<StreamingChatMessageContent>(
                    this._prompt,
                    arguments: new(openAIPromptExecutionSettings) {
                        { "messages", chatMessages }
                    },
                    cancellationToken);

            // Print the chat completions
            ChatMessageContent? chatMessageContent = null;

            // TODO: This should be a Mediatr event
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
                chatMessageContent!.Content += content.Content;

                // TODO: This should be using Mediatr
                await this._eventHandler.AppendResponseAsync(content.Content, cancellationToken)
                    .ConfigureAwait(false);

            }
            // TODO: This should be using Mediatr
            await this._eventHandler.EndResponseAsync(cancellationToken).ConfigureAwait(false);

            chatMessages.AddMessage(chatMessageContent!);
        }
    }
}
