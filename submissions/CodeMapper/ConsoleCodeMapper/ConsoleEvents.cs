// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <inheritdoc/> 
public class ConsoleEvents : IChatEvents
{
    /// <inheritdoc/>
    public async Task<string> GetInitialPromptAsync(CancellationToken cancellationToken)
    {
        var text = await File.ReadAllTextAsync("FirstPrompt.md", cancellationToken)
                             .ConfigureAwait(false);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("First prompt: " + text);

        return text;
    }

    /// <inheritdoc/> 
    public Task AppendResponseAsync(string? text, CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(text);

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task BeginResponseAsync(CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.Write("Assistant > ");

        return Task.CompletedTask;
    }

    /// <inheritdoc/> 
    public Task EndResponseAsync(CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        return Task.CompletedTask;
    }

    /// <inheritdoc/> 
    public Task<string> GetAskAsync(CancellationToken cancellationToken = default)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("User > ");
        string? ask = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(ask))
        {
            return Task.FromResult("Proceed");
        }

        return Task.FromResult(ask);
    }
}
