# Interactive History Explorer ChatGPT Plugin

An interactive plugin that uses SK and GPT to create personalised, immersive journeys through different historical eras, complete with interactive dialogues and historical trivia.

This project contains the code for a ChatGPT plugin. It includes the following components:

- An endpoint that serves up an ai-plugin.json file for ChatGPT to discover the plugin
- A generator that automatically converts prompts into semantic function endpoints
- The ability to add additional native functions as endpoints to the plugin

## Hackathon Submission

The non-hackathon repo can be found [here](https://github.com/jcreek/HistoryExplorerChatGPTPlugin).

This plugin was created as a submission for the second Microsoft Semantic Kernel hackathon, which ran from December 4th to 11th 2023. The aim of the hackathon was to get the community using the release candidates for Semantic Kernel, however an additional challenge came from the three release candidates being released with breaking changes partway through the week, and no documentation being available for using the new features in this project type. As such, this project is a proof of concept for the idea of using SK to create interactive historical narratives, but is not a fully functional project.

As part of this project, I ended up updating the example project codebase, [firstly to dotnet 8 and SKv1 preview 8](https://github.com/MicrosoftDocs/semantic-kernel-docs/pull/69) and [finally to SKv1 RC3](https://github.com/MicrosoftDocs/semantic-kernel-docs/pull/71) to enable others to more easily create their plugins without having to work out how to update the example code themselves.

There are six semantic skills provided to this plugin for the planner to access, to enable the creation of interactive historical narratives. These are:

### 1. ADayInTheLifeOfAHistoricalFigure

Creates a detailed day-in-the-life story of a historical figure.

e.g. input "Julius Caesar"

> Julius Caesar, the renowned Roman general and statesman, begins his day at dawn with a rigorous exercise routine, preparing his body and mind for the challenges ahead. As the day progresses, he attends to his administrative duties, strategizing military campaigns and managing the affairs of the Roman Republic. Despite facing political opposition and conspiracies, Caesar's charismatic personality and astute leadership skills enable him to navigate the complex socio-political landscape of ancient Rome. In his personal life, Caesar enjoys spending time with his family and indulging in intellectual pursuits, such as writing and engaging in philosophical discussions. Explore Caesar's professional achievements, personal life, or the socio-political context of ancient Rome.

### 2. DiscoverHistoricalArtefacts

Constructs a narrative around the discovery of a historical artifact.

e.g. input "The Rosetta Stone"

> In 1799, during Napoleon Bonaparte's campaign in Egypt, a group of French soldiers stumbled upon a remarkable artifact known as the Rosetta Stone. This ancient slab of black basalt, inscribed with three different scripts - Egyptian hieroglyphs, Demotic script, and ancient Greek - would prove to be the key to unlocking the mysteries of ancient Egyptian civilization. The stone's discovery provided scholars with the means to decipher hieroglyphs, leading to a deeper understanding of Egypt's rich history and culture.

### 3. ExploreAnAncientCivilisation

Generates a detailed description of a day in an ancient civilization.

e.g. input "Ancient Egypt"

> Welcome to Ancient Egypt, a civilization known for its rich history and fascinating culture. Let's explore a day in the life of an ancient Egyptian.

> As the sun rises over the Nile River, the bustling city of Thebes comes to life. The streets are filled with merchants setting up their stalls in the marketplace, offering a variety of goods such as spices, textiles, and precious metals. The aroma of exotic spices fills the air, enticing passersby to stop and haggle for the best prices.

> In the heart of the city, magnificent temples and grand palaces stand as a testament to the religious and political power of the pharaoh. The architecture is awe-inspiring, with towering columns and intricate hieroglyphic carvings that depict the stories of gods and pharaohs. The people gather at these temples to pay homage to their deities, seeking blessings and guidance for the day ahead.

> In the neighborhoods, families engage in their daily routines. Women weave colorful fabrics on looms, creating beautiful garments adorned with intricate patterns. Men work in fields, tending to crops such as wheat and barley, while children play games in the streets, their laughter echoing through the city.

> Ancient Egyptians valued education, and children attend schools where they learn to read and write hieroglyphics. The scribes, highly respected members of society, record important events and maintain the kingdom's administrative records.

> In the evening, families gather for meals, enjoying a variety of dishes such as bread, vegetables, and fish from the Nile. Music and dance fill the air as entertainers perform for the enjoyment of all. The night sky, free from light pollution, reveals a breathtaking display of stars, guiding the ancient Egyptians in their belief that the gods watched over them.

> Ancient Egypt was a civilization deeply rooted in tradition and spirituality. Its people embraced a strong sense of community and reverence for their gods. Exploring the marketplaces, admiring the architecture, and experiencing the local customs would provide a glimpse into the vibrant and fascinating daily life of the ancient Egyptians.

### 4. ExploreAncientTechnologies

Generates an explanation about an ancient technology.

e.g. input "Roman Concrete"

> Roman concrete, also known as opus caementicium, was an ancient technology invented by the Romans around 300 BC. It was a revolutionary building material that consisted of a mixture of lime, volcanic ash, and aggregate, such as stones or bricks. This concrete was used extensively in the construction of Roman buildings, roads, and aqueducts, and its durability and strength allowed for the creation of large and complex structures that still stand today.

> Roman concrete was ahead of its time due to its unique composition and engineering properties. Unlike modern concrete, which relies on steel reinforcement for strength, Roman concrete gained its strength from the chemical reaction between the lime and volcanic ash, making it more resistant to cracking and erosion. This innovation allowed the Romans to construct massive structures, such as the Pantheon and the Colosseum, which still inspire awe today.

> The influence of Roman concrete on future innovations cannot be overstated. Its durability and longevity set a standard for construction materials that would be emulated and improved upon in later centuries. The knowledge of Roman concrete was lost during the Middle Ages but was rediscovered during the Renaissance, leading to the development of modern concrete. Today, concrete is one of the most widely used construction materials in the world, and its invention can be traced back to the ancient Romans and their pioneering use of opus caementicium.

> To deepen your understanding of Roman concrete, you can explore how the Romans utilized different types of aggregate to enhance its properties, or learn about the specific techniques they employed to create structures that have withstood the test of time. Additionally, you can delve into the architectural and engineering advancements made possible by Roman concrete, such as the construction of domes and vaults.

### 5. JourneyThroughAnHistoricalEvent

Creates an interactive story based on a historical event.

e.g. input "The Battle of Hastings"

> As the sun rose over the English countryside, the armies of William the Conqueror and Harold II faced each other on the battlefield. With the fate of England hanging in the balance, the clash of swords and the cries of warriors filled the air. In a pivotal moment, an arrow struck Harold in the eye, leading to his demise and the ultimate victory of William. The Battle of Hastings in 1066 marked the end of Anglo-Saxon rule and the beginning of Norman dominance in England, forever altering the course of history.

> Now, it's your turn to explore different perspectives and outcomes of this significant event.

> 1. Choose to follow the story from the perspective of William the Conqueror and witness his strategic decisions that led to victory.
> 2. Choose to follow the story from the perspective of Harold II and experience the challenges he faced in defending his kingdom.
> 3. Choose to explore the aftermath of the battle and its impact on the English people and their way of life.

### 6. MeetHistoricalFigures

Generates a conversation with a historical figure.

e.g. input "Abraham Lincoln"

> User: Hello, Mr. Lincoln! It's an honor to have the opportunity to speak with you. I've always been fascinated by your life and the impact you had on American history. Can you tell me a bit about your early years and how you became involved in politics?

> Abraham Lincoln: Thank you for your kind words. I was born on February 12, 1809, in a log cabin in Kentucky. My family later moved to Indiana and then settled in Illinois. Growing up, I had limited formal education, but I was an avid reader and self-taught. I worked various jobs, including as a storekeeper and a postmaster, before studying law and becoming a lawyer. My interest in politics began when I served in the Illinois state legislature in the 1830s.

> User: That's impressive, Mr. Lincoln. I've always admired your perseverance and dedication to self-improvement. Your presidency is often associated with the American Civil War. Can you tell me more about your views on slavery and how they evolved over time?

> Abraham Lincoln: Certainly. Slavery was a deeply divisive issue during my time. While I personally abhorred slavery, I initially believed that the federal government did not have the power to abolish it in states where it already existed. However, I strongly opposed its expansion into new territories. Over time, my views evolved, and I came to believe that slavery was morally wrong and should be abolished. This led to the Emancipation Proclamation in 1863, which declared that all slaves in Confederate territory were to be set free.

> User: Your Emancipation Proclamation was a significant step towards ending slavery, Mr. Lincoln. How did you manage to navigate the challenges of leading the country during such a tumultuous time?

> Abraham Lincoln: Leading the country during the Civil War was undoubtedly one of the most challenging periods in American history. I faced immense pressure and criticism from both sides of the conflict. However, I believed in preserving the Union and ensuring that all Americans were treated equally. I surrounded myself with a diverse cabinet and sought advice from various perspectives. I also communicated directly with the American people through speeches and letters, aiming to maintain their support and understanding.

> User: Your leadership and ability to unite the country during such a divisive time are truly remarkable, Mr. Lincoln. Looking back, what do you consider to be your most significant achievement as President?

> Abraham Lincoln: Thank you for your kind words. While the Emancipation Proclamation was a pivotal moment, I believe that my most significant achievement was the passage of the Thirteenth Amendment to the United States Constitution. This amendment, ratified in 1865, abolished slavery and involuntary servitude throughout the entire country. It was a crucial step towards ensuring equality and freedom for all Americans, regardless of their race or background.

> User: Your impact on American history is undeniable, Mr. Lincoln. Thank you for sharing your insights and for your dedication to the principles of equality and justice. Is there anything else you would like to add or discuss?

> Abraham Lincoln: Thank you for your kind words. I would like to emphasize the importance of unity and understanding in times of division. It is crucial to remember that we are all part of the same nation and that progress can only be achieved through cooperation and empathy. I hope that my legacy serves as a reminder of the power of leadership and the potential for positive change.

## Future Enhancements

This project makes use of whatever planner is the default for semantic skills in this type of project. Ideally it would make use of the new handlebars planner for generating multi-step plans, which uses Handlebars templates. This is instrumental in structuring historical narratives and interactions. For example, generating a story about a specific historical event or era. Unfortunately, with the release candidate releasing partway through the hackathon and a lack of documentation I was unable to work out how to switch to the new planner.

Likewise, an assistant persona could be created to make GPT respond more like an old storyteller or historian, but this was also not possible due to the lack of documentation. The plan was to utilize templated assistant instructions for dynamically creating stories based on the historical context and user inputs.

It would also be good to add access to an historical API to enable the plugin to provide more detailed information about historical events, figures, and technologies. This would allow the plugin to provide more detailed information about the historical context of the user's questions and choices, and to provide more detailed information about the historical figures and events that the user is interacting with. Currently it is relying entirely on GPT's awareness of history, which is not bad but could definitely be better.

Currently users can select time periods, ask questions, and interact with the narrative. To further build the time travel experience it would be good to hence this further by employing handlebars templates to structure the narratives dynamically, allowing for user choices to influence the story flow. This would make the experience more immersive and interactive, and would allow for more complex narratives to be created.

## Prerequisites

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) is required to run this project.
- [Azure Functions Core Tools](https://www.npmjs.com/package/azure-functions-core-tools) is required to run this project.
- Install the recommended extensions
  - [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
  - [Semantic Kernel Tools](https://marketplace.visualstudio.com/items?itemName=ms-semantic-kernel.semantic-kernel)

## Configuring the project

To configure the project, you need to provide the following information:

- Define the properties of the plugin in the [appsettings.json](./azure-function/appsettings.json) file.
- Enter the API key for your AI endpoint in the [local.settings.json](./azure-function/local.settings.json) file.

### Using appsettings.json

Configure an OpenAI endpoint

1. Copy [settings.json.openai-example](./config/appsettings.json.openai-example) to `./appsettings.json`
2. Edit the `kernel` object to add your OpenAI endpoint configuration
3. Edit the `aiPlugin` object to define the properties that get exposed in the ai-plugin.json file

Configure an Azure OpenAI endpoint

1. Copy [settings.json.azure-example](./config/appsettings.json.azure-example) to `./appsettings.json`
2. Edit the `kernel` object to add your Azure OpenAI endpoint configuration
3. Edit the `aiPlugin` object to define the properties that get exposed in the ai-plugin.json file

### Using local.settings.json

1. Edit the `Values` object to add your OpenAI endpoint configuration in the `apiKey` property

## Running the project

To run the Azure Functions application just hit `F5`.

To build and run the Azure Functions application from a terminal use the following commands:

```powershell
cd azure-function
dotnet build
cd bin/Debug/net6.0
func host start  
```

If you want to preview it in an actual ChatGPT-like experience, use [Chat CoPilot](https://github.com/microsoft/chat-copilot) and point it to the local endpoint at `http://localhost:7071/.well-known/ai-plugin.json` to add the plugin while this project is running.
