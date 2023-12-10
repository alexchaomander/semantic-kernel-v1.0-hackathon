// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <inheritdoc/>
public interface IChatEvents
{
    /// <inheritdoc/>
    public Task<string> GetInitialPromptAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task<string> GetAskAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task BeginResponseAsync(CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task AppendResponseAsync(string? text, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public Task EndResponseAsync(CancellationToken cancellationToken = default);
}
