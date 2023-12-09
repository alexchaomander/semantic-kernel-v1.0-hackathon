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
    public async Task<string> LoadCategoryCSVAsync(
        [Description("The input file name. It cannot contain a path.")]
        string fileName,
        CancellationToken cancellationToken)
    {
        var categories = await this.LoadCSVAsync<CategoryItem>(fileName, cancellationToken).ConfigureAwait(false);

        var json = JsonSerializer.Serialize(categories);

        this._logger!.LogDebug("{MethodName}({FileName})", nameof(LoadCategoryCSVAsync), fileName);

        return json;
    }

    /// <inheritdoc />
    [KernelFunction, Description(
        "Returns the codes for the given category from a comma delimited file (csv) containing codes. " +
        "It returns a JSON array of codes.")]
    public async Task<string> GetCodesOfCategoryFromCSVAsync(
        [Description("The input file name. It cannot contain a path.")]
        string fileName,
        [Description("The category identifier.")]
        string categoryId,
        CancellationToken cancellationToken)
    {
        if (categoryId == "4")
        { }

        var codes = await this.LoadCSVAsync<CodeItem>(fileName, cancellationToken).ConfigureAwait(false);

        var filteredCodes = codes
            .Where(c => c.CategoryId == categoryId)
            .OrderBy(c => c.Id);

        var json = JsonSerializer.Serialize(filteredCodes);

        this._logger!.LogDebug("{MethodName} {FileName} {CategoryId}", nameof(GetCodesOfCategoryFromCSVAsync), fileName, categoryId);

        return json;
    }

    /// <inheritdoc />
    [KernelFunction, Description(
        "Returns the categories referenced in a comma delimited file (csv) containing codes. " +
        "It returns a JSON array of codes.")]
    public async Task<string> GetCategoriesReferencedInCSVAsync(
        [Description("The input file name.")]
        string fileName,
        CancellationToken cancellationToken)
    {
        var codes = await this.LoadCSVAsync<CodeItem>(fileName, cancellationToken).ConfigureAwait(false);

        var categories = codes.Select(c => new CategoryItem
        {
            Id = c.CategoryId,
            Description = c.CategoryDescription,
        })
        .DistinctBy(x => x.Id)
        .OrderBy(c => c.Id);

        var json = JsonSerializer.Serialize(categories);

        this._logger!.LogDebug("{MethodName} {FileName} {CategoryId}", nameof(GetCategoriesReferencedInCSVAsync), fileName);

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

        this._logger!.LogDebug("Loaded {File}. {RecordCount} record(s) found.", fileName, values.Count);

        return values;
    }
}
