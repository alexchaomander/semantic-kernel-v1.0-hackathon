// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

public class CategoryItem
{
    public string Id { get; set; }
    public string Abbreviation { get; set; }
    public string Description { get; set; }
}

public class CodeItem
{
    public string Id { get; set; }
    public string CategoryId { get; set; }
    public string CategoryDescription { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
}

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
    [KernelFunction, Description("Loads a comma delimited file (csv) containing code categories. It returns a JSON array of categories.")]
    public async Task<string> LoadCategoriesCSVAsync(
        [Description("The input file name. It cannot contain a path.")]
        string fileName,
        CancellationToken cancellationToken)
    {
        var categories = await this.LoadCSVAsync<CategoryItem>(fileName, cancellationToken).ConfigureAwait(false);

        var json = JsonSerializer.Serialize(categories);
        return json;
    }

    /// <inheritdoc />
    [KernelFunction, Description("Loads a comma delimited file (csv) containing codes. It returns a JSON array of codes.")]
    public async Task<string> LoadCodesCSVAsync(
        [Description("The input file name. It cannot contain a path.")]
        string fileName,
        CancellationToken cancellationToken)
    {
        var codes = await this.LoadCSVAsync<CodeItem>(fileName, cancellationToken).ConfigureAwait(false);

        var json = JsonSerializer.Serialize(codes);
        return json;
    }

    private async Task<IEnumerable<T>> LoadCSVAsync<T>(string fileName, CancellationToken cancellationToken)
        where T : class
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
        };

        using var reader = new StreamReader(fileName);
        using var csv = new CsvReader(reader, configuration);

        var values = new List<T>();
        await foreach (var record in csv.GetRecordsAsync<T>(cancellationToken))
        {
            values.Add(record);
        }

        this._logger!.LogInformation("Loaded {File}. {RecordCount} record(s) found.", fileName, values.Count);

        return values;
    }
}
