using System.Text.RegularExpressions;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;

namespace SecureGenMask.Plugin.RedactPlugin;

public class RedactPlugin
{
    private readonly ILogger<RedactPlugin> _logger;
    private readonly IConfiguration _configuration;
    
    public RedactPlugin(ILogger<RedactPlugin> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    [KernelFunction, System.ComponentModel.Description("Used to Redact PII and Sensitive information.")]
    public async Task<string> RedactPII([System.ComponentModel.Description("The content to redact")] string content)
    {

        if (!_configuration.GetValue<bool>("Use_Azure_For_Redaction"))
        {
            return RedactSensitiveInfo(content);
        }
        
        var credentials = _configuration.GetValue<string>("Azure_CognitiveService_KeyCredential");
        var endpoint = _configuration.GetValue<string>("Azure_CognitiveService_Endpoint");
 
     
        var client = new TextAnalyticsClient( new Uri(endpoint), new AzureKeyCredential(credentials));
     
        
        PiiEntityCollection entities = await client.RecognizePiiEntitiesAsync(content);
        
        _logger.LogInformation("Redacted Text: {entities.RedactedText}", entities.RedactedText);
        if (entities.Count > 0)
        {
            _logger.LogInformation($"Used Azure to Recognize {entities.Count} PII", entities.Count);
        }
        else
        {
         _logger.LogInformation("No PII were found.");
        }

        return entities.RedactedText;
    }
    
    
    
    public static string RedactSensitiveInfo(string text)
    {
        Console.WriteLine("Using local redact..");
        // Redact email addresses
        text = Regex.Replace(text, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b", "[REDACTED_EMAIL]");

        // Redact phone numbers (supports common formats)
        text = Regex.Replace(text, @"\b(\+\d{1,2}\s?)?(\(\d{1,4}\)\s?)?\d{1,5}[-.\s]?\d{1,5}[-.\s]?\d{1,5}\b", "[REDACTED_PHONE]");

        // Redact Social Insurance Numbers (SIN)
        text = Regex.Replace(text, @"\b\d{3}-\d{3}-\d{3}\b", "[REDACTED_SIN]");

        // Redact Social Security Numbers (SSN)
        text = Regex.Replace(text, @"\b\d{3}-\d{2}-\d{4}\b", "[REDACTED_SSN]");

        // Redact dates of birth (supports common formats)
        text = Regex.Replace(text, @"\b\d{1,2}/\d{1,2}/\d{4}\b", "[REDACTED_DOB]");

        // Redact Vehicle Identification Numbers (VIN)
        text = Regex.Replace(text, @"\b[A-HJ-NPR-Z0-9]{17}\b", "[REDACTED_VIN]");

        return text;
    }
}