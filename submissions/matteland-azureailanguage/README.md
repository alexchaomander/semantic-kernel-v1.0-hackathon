# MattEland.AI.Semantic.Plugins.TextAnalytics
A Semantic Kernel Plugin that uses Azure AI Language to analyze the input text and return back information.

## Repository
[IntegerMan/MattEland.AI.Semantic.Plugins.TextAnalytics](https://github.com/IntegerMan/MattEland.AI.Semantic.Plugins.TextAnalytics)

## Description
This plugin is designed to be used with Semantic Kernel and provides native functions for analyzing text using Azure AI Language services via the Azure.AI.TextAnalytics NuGet package.

The plugin provides the following functions:

- `AnalyzeSentiment` - Analyzes the sentiment of the input text and returns a score between 0 and 1.
- `SummarizeText` - Uses abstractive summarization to summarize the input text.
- `SummarizeTextWithExtracts` - Uses abstractive and extractive summarization to summarize the input text and provide relevant sentence extracts.
- `IdentifyEntities` - Identifies entities in the input text and returns a list of entities via EntityRecognition and LinkedEntityRecognition.
- `IdentifySensitiveInformation` - Identifies sensitive information in the input text and returns the strings that were identified.

To use, instantiate the plugin by providing it the necessary information to authenticate with your Azure AI service or Azure AI Language resource using one of the constructor overloads. Next, import the plugin into the Kernel and it will be available for planners to invoke to fulfill any language-related requests.

*Note: This plugin was designed under beta 8 and updated to follow the v1.0.0 rc3 conventions after completion*

*Note: This plugin is adapted off of some code I'm using in the [Semantic Kernel workshop](https://codemash.org/session-details/?id=538027) I'm running at [CodeMash 2024](https://CodeMash.org) in January. That workshop starts by talking about Azure AI services, then moves to Azure OpenAI and then Semantic Kernel. By providing attendees with a plugin that looks similar to the Azure AI Language code we started with, I hoope to show them that SK can integrate a wide variety of capabilities, including other AI capabilities.*

## Nuget Package
This repository is not yet available via NuGet, but should be by the end of 2023 as I clear some of my December speaking and content obligations.

## Contact Info
Share your contact info so the SK team and the community can get in touch

**Name:** Matt Eland
**Email:** Matt@MattOnDataScience.com
