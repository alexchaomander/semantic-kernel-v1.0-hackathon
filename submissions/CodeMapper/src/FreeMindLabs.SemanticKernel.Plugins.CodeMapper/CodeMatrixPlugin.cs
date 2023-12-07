// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

public class TestDI
{
    private readonly ILogger<TestDI> _logger;

    /// <inheritdoc />
    public TestDI(ILogger<TestDI> logger)
    {
        this._logger = logger;
    }
}

/// <inheritdoc />
public class CodeMatrixPlugin
{
    private readonly ILogger<CodeMatrixPlugin>? _logger;
    private readonly TestDI _testDI;

    /// <inheritdoc />
    public CodeMatrixPlugin(ILogger<CodeMatrixPlugin> logger, TestDI testDI)
    {
        //this._logger = ActivatorUtilities.CreateInstance<ILogger<CodeMatrixPlugin>>();
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this._testDI = testDI ?? throw new ArgumentNullException(nameof(testDI));
    }

    /// <inheritdoc />
    [KernelFunction, Description("Loads an Matrix spreadsheet file from disk and indexes its contents. Returns a json string.")]
    public async Task<string> LoadSpreadsheetAsync(string fileName, string pageName)
    {
        this._logger!.LogInformation("Opening spreadsheet {File}, page name {PageName}", fileName, pageName);

        await Task.Delay(100).ConfigureAwait(false);

        return $"Failed to load '{fileName}'.";
    }
}
