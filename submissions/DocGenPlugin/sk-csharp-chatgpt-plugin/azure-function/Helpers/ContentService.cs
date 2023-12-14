using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AI.OpenAI;
using Models;

namespace Helpers
{
    public class ContentService
    {
        public string SkillName { get; set; } = "ContentSkill";
        public string FunctionName { set; get; } = "Content";
        int MaxTokens { set; get; }
        double Temperature { set; get; }
        double TopP { set; get; }
        public bool IsProcessing { get; set; } = false;

        Dictionary<string, KernelFunction> ListFunctions = new Dictionary<string, KernelFunction>();

        Kernel kernel { set; get; }

        public ContentService()
        {
            // Configure AI backend used by the kernel
            var setting = AppSettings.LoadSettings();
            kernel = new KernelBuilder()
       .WithChatCompletionService(setting.Kernel)
       .Build();

            SetupSkill();
        }

        public void SetupSkill(int MaxTokens = 2000, double Temperature = 0.2, double TopP = 0.5)
        {
            this.MaxTokens = MaxTokens;
            this.Temperature = Temperature;
            this.TopP = TopP;

            string skPrompt = """
Instruction:
{{$input}}

Generate content from instruction above, page number can be 1 to 20 adjust with content length per page, generate with json format like this:
[
{
    "page" : "1",
    "content" : "[generate content here]"
},
{
    "page" : "2",
    "content" : "[generate content 2 here]"
},
]

Content:
""";

            var fun = kernel.CreateFunctionFromPrompt(skPrompt, new OpenAIPromptExecutionSettings() { MaxTokens = MaxTokens, Temperature = Temperature, TopP = TopP });
            ListFunctions.Add(FunctionName, fun);
        }

        public async Task<string> GenerateContent(string input)
        {
            string Result = string.Empty;
            if (IsProcessing) return Result;

            try
            {
              
                IsProcessing = true;
                var arg = new KernelArguments()
                {
                    ["input"] = input
                };
                var Content = await kernel.InvokeAsync(ListFunctions[FunctionName],arg);

                Console.WriteLine(Content);
                Result = Content.GetValue<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return ex.ToString();
            }
            finally
            {
                IsProcessing = false;
            }
            return Result;
        }

    }
}
