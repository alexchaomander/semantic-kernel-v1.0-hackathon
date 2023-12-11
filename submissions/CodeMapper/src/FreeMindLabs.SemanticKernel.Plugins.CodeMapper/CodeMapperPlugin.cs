// Semantic Kernel Hackathon 2 - Code Mapper by Free Mind Labs.

using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace FreeMindLabs.SemanticKernel.Plugins.CodeMapper;

/// <inheritdoc />
public class CodeMapperPlugin
{
    private readonly ILogger<CodeMapperPlugin>? _logger;

    /// <inheritdoc />
    public CodeMapperPlugin(ILogger<CodeMapperPlugin> logger)
    {
        this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    /// <inheritdoc />
    [KernelFunction, Description(
        "Returns the categories used in the (csv) file containing codes. " +
        "The result is a JSON array of categories with the fields: Id, Description.")]
    public async Task<string> ReadCodesFromCSVAsync(
        [Description("The input file name. It cannot contain a path.")]
        string fileName,
        [Description("n, optional filter for the category id. Setting this value to a valid category id will return just the codes of that category.")]
        string? categoryId,
        CancellationToken cancellationToken)
    {
        var codes = await this.LoadCSVAsync<CodeItem>(fileName, cancellationToken).ConfigureAwait(false);

        var source = ((categoryId is null) ? codes : codes.Where(c => c.CategoryId == categoryId))
            .OrderBy(c => c.Id)
            .ToList();

        var json = this.SerializeToJson(source);

        this._logger!.LogDebug("Loaded codes from {FileName}", fileName);

        return json;
    }

    /// <inheritdoc />
    [KernelFunction, Description(
        "Returns the categories referenced in a csv file containing codes. " +
        "It returns a JSON array of categories with the fields: Id, Description.")]
    public async Task<string> ReadCategoriesFromCSVAsync(
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

        var json = this.SerializeToJson(categories);

        this._logger!.LogDebug("Loaded categories from {FileName}", fileName);

        return json;
    }

    private async Task<IEnumerable<T>> LoadCSVAsync<T>(string fileName, CancellationToken cancellationToken)
        where T : class
    {
        fileName = this.GetCorrectFileName(fileName);

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

        this._logger!.LogDebug("Loaded CSV {FileName}. {RecordCount} record(s).", fileName, values.Count);

        return values;
    }

    private JsonSerializerOptions _jsonSerializerOptions = new()
    {
        WriteIndented = true,
    };

    private string SerializeToJson<T>(T value)
    {
        var json = JsonSerializer.Serialize(value, this._jsonSerializerOptions);

        return json;
    }

    private string GetCorrectFileName(string fileName)
    {
        var fullName = Path.Combine("Data", fileName);
        fullName = Path.ChangeExtension(fullName, ".csv");
        if (!File.Exists(fullName))
        {
            throw new FileNotFoundException($"File not found: {fullName}");
        }

        return fullName;
    }
}
