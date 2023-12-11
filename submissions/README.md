# MattEland.AI.Semantic.Plugins.TextAnalytics
A Semantic Kernel Plugin that uses Azure AI Language to analyze the input text and return back information.

This plugin is designed to be used with Semantic Kernel and provides native functions for analyzing text using Azure AI Language services via the Azure.AI.TextAnalytics NuGet package.

The plugin provides the following functions:

- `AnalyzeSentiment` - Analyzes the sentiment of the input text and returns a score between 0 and 1.
- `SummarizeText` - Uses abstractive summarization to summarize the input text.
- `SummarizeTextWithExtracts` - Uses abstractive and extractive summarization to summarize the input text and provide relevant sentence extracts.
- `IdentifyEntities` - Identifies entities in the input text and returns a list of entities via EntityRecognition and LinkedEntityRecognition.
- `IdentifySensitiveInformation` - Identifies sensitive information in the input text and returns the strings that were identified.
