## Hackathon #1 Winners

Marketplace for LLM Assets Plugin (EmbedElite) | Semantic Kernel Plugin Hackathon #1 Winner
https://www.youtube.com/watch?v=CmRMoU99OGU
https://github.com/embedelite/sk-hackathon

Natural Language to SQL | Semantic Kernel Plugins Hackathon #1 Winner!
https://www.youtube.com/watch?v=s8w_GHvZ558
https://github.com/anthonypuppo/sk-nl2ef-plugin

Pokémon Text Adventure Plugin! | Semantic Kernel Plugin Hackathon
https://www.youtube.com/watch?v=C-UhdzCwfhY
https://github.com/jcreek/PokemonAdventureChatGPTPlugin

### misc

```

#region Old code that caused a diff service collection to be used.
//services.AddTransient<Kernel>(sp =>
//{
//    var builder = new KernelBuilder()
//        .AddOpenAIChatCompletion("gpt-3.5-turbo", configuration["OpenAI:ApiKey"]!);
//
//    
//    // Q1: Does not support logger factory, so we have to duplicate the configuration here.
//    builder.Services.AddLogging(cfg => cfg.AddConsole());
//
//    var testDIInstance = sp.GetRequiredService<TestDI>();
//    builder.Services.AddSingleton(testDIInstance);
//
//    var kernel = builder.Build();
//    //kernel.ImportPluginFromObject<CodeMatrixPlugin>();
//    kernel.ImportPluginFromType<CodeMatrixPlugin>();
//    return kernel;
//});
#endregion
```