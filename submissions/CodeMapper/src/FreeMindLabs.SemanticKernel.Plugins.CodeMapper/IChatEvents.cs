﻿// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <summary>
/// This interface defines the events that are used by the <see cref="InteractiveChat"/> class.
/// A better approach would have been to use MediatR, but I didn't want to add another dependency and I ran out of time.
/// </summary>
public interface IChatEvents
{
    /// <summary>
    /// Returns the initial prompt to be used by the chat.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetInitialPromptAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asks the user for input.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<string> GetAskAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Called when AI is about to respond.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task BeginResponseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Called for each chunk of text generated by AI.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task AppendResponseAsync(string? text, CancellationToken cancellationToken = default);

    /// <summary>
    /// Called when AI has finished responding.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task EndResponseAsync(CancellationToken cancellationToken = default);
}
