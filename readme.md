# TimHanewich.Foundry
![banner](https://i.imgur.com/URuvzhE.png)

![Framework](https://img.shields.io/badge/Framework-.NET%2010.0-512bd4)
![NuGet Version](https://img.shields.io/nuget/v/TimHanewich.Foundry)

**TimHanewich.Foundry** is a lightweight .NET library designed specifically for interfacing with LLM deployments in Microsoft Foundry (formerly *Azure AI Foundry*). It streamlines the process of communicating with Foundry services, providing a clean, strongly-typed interface for modern .NET applications.

### Key Features
- 🔑 **Flexible Auth**: Native support for both API Key and Microsoft Entra ID (Service Principals).
- 🧵 **Smart Context**: Simple conversation tracking using the responses API's `previous_response_id`.
- 🛠️ **Native Tooling**: Strongly-typed classes for Function Calling and tool result handling.
- 📦 **Structured Data**: Built-in support for JSON Mode for structured model outputs.
- 🚀 **Modern .NET**: Built from the ground up to support the latest .NET 10+ features.

### Why use this over the standard OpenAI SDK?
While generic SDKs exist, **TimHanewich.Foundry** is purpose-built for the unique nuances of the Microsoft Foundry ecosystem. It simplifies the authentication handshake for Entra ID and provides a flatter, more intuitive object model for developers who want to get up and running without the overhead of enterprise-scale boilerplate.

## Installing
`TimHanewich.Foundry` is [available on Nuget](https://www.nuget.org/packages/TimHanewich.Foundry)! Installing is easy, simply run:
```
dotnet add package TimHanewich.Foundry
```

## Example Use
Below are some examples on how to use this library:

### Basic Prompting
Below shows the basic set up and prompting process:
```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
rr.Inputs.Add(new Message(Role.developer, "Talk like a cowboy.")); //system prompt
rr.Inputs.Add(new Message(Role.user, "Hi! Why is the sky blue?")); //user prompt

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
}
```

This will result in:
```
Response ID: resp_05c65468f65bdb3c006950294d66948196ac0afea12bfba22d
Input tokens consumed: 79
Output tokens consumed: 374
Response: Howdy partner — reckon the sky’s blue ‘cause of a little thing called Rayleigh scatterin’. Sunlight’s made up of all colors, but when it hits the tiny air molecules up yonder, the shorter wavelengths (blues and violets) get scattered much more than the longer reds. The amount scattered goes up steep-like with shorter wavelength (about 1 over wavelength to the fourth power), so blue light gets tossed around all over the place and fills the sky.

Now, you might ask why it don’t look violet if violet scatters even more — well, the Sun gives off less violet light, and our eyes ain’t as keen on violet, so blue wins out. At sunrise and sundown the sunlight travels through more air, scatterin’ away the blues and lettin’ the reds and oranges ride in, which is why them sunsets are fiery.
```

### Follow-up Message
To then continue the conversation with a follow up message, you must specify the `previous_response_id` property:
```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
rr.PreviousResponseID = "resp_05c65468f65bdb3c006950294d66948196ac0afea12bfba22d"; //previous response ID specifies the conversation history
rr.Inputs.Add(new Message(Role.user, "I'm still not getting it. Can you explain it to me like I am 5 years old?")); //user message

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
}
```

This will result in:
```
Response ID: resp_05c65468f65bdb3c00695029d682908196a7ce63e0b59f62aa
Input tokens consumed: 285
Output tokens consumed: 497
Response: Well howdy, little pardner. Imagine sunlight is a big box o’ crayons with every color in it. When that light comes down through the air, tiny invisible things in the sky like to bump the colors around. The blue crayons are like tiny, bouncy marbles that get bumped and scattered every which way, so blue fills the whole sky for us to see. The red crayons are bigger and don’t get bounced around as much, so they mostly keep goin’ straight.

When the sun is risin’ or settin’, its light has to travel through lots more air, so most of the blue marbles get scattered away before they reach our eyes — that’s why the sky looks orange and red then. And don’t worry ‘bout violet; our eyes don’t see it as well, so blue looks brightest to us.
```

### Function Calling
You can also achieve function-calling functionality (a.k.a. tool calling) like so:
```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
rr.Inputs.Add(new Message(Role.user, "What is the weather in 98004?")); //user message

//Add the "CheckWeather" tool as a tool (function) the model has available to it
Tool CheckWeather = new Tool();
CheckWeather.Name = "CheckWeather";
CheckWeather.Description = "Check the weather for any zip code.";
CheckWeather.Parameters.Add(new ToolInputParameter("zip_code", "Zip code of the area you want to check the weather for"));
rr.Tools.Add(CheckWeather);

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
    else if (exchange is ToolCall tc) //if it is a tool call
    {
        Console.WriteLine();
        Console.WriteLine("Tool call received:");
        Console.WriteLine("Tool Name: " + tc.ToolName);
        Console.WriteLine("Tool Call ID: " + tc.CallId);
        Console.WriteLine("Arguments: " + tc.Arguments.ToString(Formatting.None));
    }
}
```

This will result in:
```
Response ID: resp_0c5335a67e04df960069502ab72a108194bffc93b794cc1a97
Input tokens consumed: 71
Output tokens consumed: 22

Tool call received:
Tool Name: CheckWeather
Tool Call ID: call_GYUF82w0DDdrV3Yf1YJo22OW
Arguments: {"zip_code":"98004"}
```

### Providing a Tool Call Output (Result)
After the model decides to make a tool call, you must provide it with the *result* of the tool call. After getting that result, you provide the result like so:

```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
rr.PreviousResponseID = "resp_0c5335a67e04df960069502ab72a108194bffc93b794cc1a97"; //previous response ID (the response that contained the tool call) - this provides conversational history!

//Add the results of the "CheckWeather" tool 
rr.Inputs.Add(new ToolCallOutput("call_GYUF82w0DDdrV3Yf1YJo22OW", "{'temperature': 72.4, 'humidity': 55.4, 'precipitation_inches': 2.4}"));

//Add the "CheckWeather" tool as a tool (function) the model has available to it
Tool CheckWeather = new Tool();
CheckWeather.Name = "CheckWeather";
CheckWeather.Description = "Check the weather for any zip code.";
CheckWeather.Parameters.Add(new ToolInputParameter("zip_code", "Zip code of the area you want to check the weather for"));
rr.Tools.Add(CheckWeather);

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
    else if (exchange is ToolCall tc) //if it is a tool call
    {
        Console.WriteLine();
        Console.WriteLine("Tool call received:");
        Console.WriteLine("Tool Name: " + tc.ToolName);
        Console.WriteLine("Tool Call ID: " + tc.CallId);
        Console.WriteLine("Arguments: " + tc.Arguments.ToString(Formatting.None));
    }
}
```

It is not shown above, but you can also specify if each parameter is required or not when declaring each parameter as a `ToolInputParameter`

This will result in:
```
Response ID: resp_0c5335a67e04df960069502bbd47f88194810ea8e510dba891
Input tokens consumed: 180
Output tokens consumed: 91
Response: Here’s the current weather for ZIP code 98004:

- Temperature: 72.4°F
- Humidity: 55.4%
- Precipitation (recent/accumulated): 2.4 inches

If you’d like a forecast (hourly or 7-day), current conditions summary (wind, sky description), or conversion to °C, tell me which and I’ll pull that for you.
```

### Getting Structured Outputs ("JSON Mode")
You can request a structured output, as JSON, like so:
```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini-testing"; //the name of your particular deployment in Foundry
rr.Inputs.Add(new Message(Role.user, "Parse out the first and last name and provide it to me in JSON like this format, as an example: {'first': 'Ron', 'last': 'Weasley'}.\n\n'Hi, my name is Harold Gargon."));
rr.RequestedFormat = ResponseFormat.JsonObject; //specify you want a JSON object output ('JSON mode')

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
}
```

This will result in the following:
```
Response ID: resp_0e2cc18156f6cc050069502ce6bcd48195ba166413e74a5a6d
Input tokens consumed: 52
Output tokens consumed: 212
Response: {"first": "Harold", "last": "Gargon"}
```

Note, the OpenAI responses API also supports the `json_schema` format in which you can specify an exact schema it must conform to - but that is not supported in this library yet!

## Using Web Search
You can also use the built-in web search tool.
```C#
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Draft the response request
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini";
rr.Inputs.Add(new Message(Role.user, "What is the latest news on Lebron James?"));

//Add the web search tool (built in tool)
rr.Tools.Add(new WebSearchTool());

Response r = fr.CreateResponseAsync(rr).Result;
Console.WriteLine(JsonConvert.SerializeObject(r, Formatting.Indented));
```

## Example Use: Entra ID Authentication
In addition to supporting Foundry's **API-key** based authentication, this library also supports **keyless authentication** using Microsoft Entra ID. See [this article](https://timhanewich.medium.com/how-to-use-microsoft-foundry-via-entra-id-authentication-with-step-by-step-screenshots-f6d381d50f3a) for further information about this authentication type, its advantages, and how it works.

To authenticate:
```
using TimHanewich.Foundry;

EntraAuthenticationHandler eah = new EntraAuthenticationHandler();
eah.TenantID = "29506ab9-d072-41d6-aead-eeda9fe7e789";
eah.ClientID = "53766182-cf31-4718-b16b-c1ff0050ba8d";
eah.ClientSecret = "4r24Q~1T.hOYCtUhdAds3a~eHxw3wP_MECQjWbLZ";
TokenCredential credential = await eah.AuthenticateAsync();
```

The `TokenCredential` class handles expiration checks (generally the access token expires in ~24 hours):
```
Console.WriteLine("Token credential expires at " + credential.Expires.ToString());         // "1/3/2026 9:31:12 AM"
Console.WriteLine("Token credential is expired: " + credential.IsExpired().ToString());    // False
```

You can then provide that access token to use your model deployment like so:

```
using TimHanewich.Foundry;
using TimHanewich.Foundry.OpenAI.Responses;

//Authenticate
EntraAuthenticationHandler eah = new EntraAuthenticationHandler();
eah.TenantID = "29506ab9-d072-41d6-aead-eeda9fe7e789";
eah.ClientID = "53766182-cf31-4718-b16b-c1ff0050ba8d";
eah.ClientSecret = "4r24Q~1T.hOYCtUhdAds3a~eHxw3wP_MECQjWbLZ";
TokenCredential credential = await eah.AuthenticateAsync();

//Check if expired
Console.WriteLine("Token credential expires at " + credential.Expires.ToString());         // "1/3/2026 9:31:12 AM"
Console.WriteLine("Token credential is expired: " + credential.IsExpired().ToString());    // False

//Define the Foundry Resource
FoundryResource fr = new FoundryResource("https://myfoundry-resource.services.ai.azure.com");
fr.ApiKey = "6ElIJZ2jsMM...";

//Create a response request (uses the Responses API)
ResponseRequest rr = new ResponseRequest();
rr.Model = "gpt-5-mini"; //the name of your particular deployment in Foundry
rr.Inputs.Add(new Message(Role.user, "Why is the sky blue?"));

//Call to API service
Response r = await fr.CreateResponseAsync(rr);

//Print response info
Console.WriteLine("Response ID: " + r.Id);
Console.WriteLine("Input tokens consumed: " + r.InputTokensConsumed.ToString());
Console.WriteLine("Output tokens consumed: " + r.OutputTokensConsumed.ToString());
foreach (Exchange exchange in r.Outputs) //loop through all outputs (output could be a message, function call, etc.)
{
    if (exchange is Message msg) //if this output is a Message
    {
        Console.WriteLine("Response: " + msg.Text);
    }
}
```

## Other Resources
- [Banner, designed in PPT](https://github.com/TimHanewich/TimHanewich.Foundry/releases/download/1/banner.pptx)