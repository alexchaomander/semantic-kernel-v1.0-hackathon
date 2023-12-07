// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <inheritdoc />
public class CodeMatrixPlugin
{
    private readonly ILogger<CodeMatrixPlugin>? _logger;

    /// <inheritdoc />
    public CodeMatrixPlugin(ILogger<CodeMatrixPlugin> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    [KernelFunction, Description("Loads an Matrix spreadsheet file from disk and indexes its contents. Returns a json string.")]
    public async Task<string> LoadSpreadsheetAsync([Description("bla bla")] string fileName, string pageName)
    {
        this._logger!.LogInformation("Opening spreadsheet {File}, page name {PageName}", fileName, pageName);

        await Task.Delay(100).ConfigureAwait(false);

        return $"Failed to load '{fileName}'.";
    }
}
