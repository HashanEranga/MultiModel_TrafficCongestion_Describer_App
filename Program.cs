using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;
using System.Text.Json.Serialization;
using TrafficCongesionApp;


// build configuration to read settings from appsettings.json
var configurations = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// initialize azure openAI chat completion service with necessary configurations
AzureOpenAIChatCompletionService chatCompletionService = new(
    endpoint: configurations["AzureOpenAI:Endpoint"] ?? throw new InvalidOperationException("AzureOpenAI:Endpoint is not configured in appsettings.json"),
    apiKey: configurations["AzureOpenAI:ApiKey"] ?? throw new InvalidOperationException("AzureOpenAI:ApiKey is not configured in appsettings.json"),
    modelId: configurations["AzureOpenAI:ModelId"],
    deploymentName: configurations["AzureOpenAI:DeploymentName"] ?? throw new InvalidOperationException("AzureOpenAI:DeploymentName is not configured in appsettings.json")
);

var executionSettings = new OpenAIPromptExecutionSettings
{
    ResponseFormat = typeof(CameraResult)
};

// get all image files from "images" directory
var imageFiles = Directory.GetFiles("Images", "*.jpg");
foreach (var imageFile in imageFiles)
{
    // load images into memory as byte arrays
    Console.WriteLine($"Processing image: {imageFile}");
    byte[] bytes = await File.ReadAllBytesAsync(imageFile);

    // create chat history with an initial system message
    ChatHistory chatHistory = new(
            @"you are a traffic anlyzer AI that monitors traffic congestion images and congestion level. Heavy congestion level is when there is very little room
            between cars and vehicles are breaking. Medium congestion is when there is a lot of cars but they are not braking. 
            low traffic is when there are few cars on the road. In addition, attempt to determine if the image was taken with a malfunctioning camera by looking the distorted images."
        );

    // add user message to the chat history
    chatHistory.AddUserMessage([
            new ImageContent(bytes, "image/jpeg"),
            new TextContent("analyze the traffic congestion level in this image and determine if the camera is malfunctioning")
        ]);

    // get the chat message content from the chat completion service
    var response = await chatCompletionService.GetChatMessageContentAsync(
        chatHistory: chatHistory,
        executionSettings: executionSettings
    );

    var options = new JsonSerializerOptions()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    CameraResult result = JsonSerializer.Deserialize<CameraResult>(response.Content, options);

    Console.ForegroundColor = result.IsBroken ? ConsoleColor.Red : ConsoleColor.Green;
    Console.WriteLine($"Is Broken : {result.IsBroken}");
    Console.WriteLine($"Traffic Congestion Level : {result.TrafficCongesionLevel}");
    Console.WriteLine($"Analysis : {result.Analysis}");
    Console.ResetColor();
    Console.WriteLine(new string('-', 40));

    // add a delay to avoid hitting rate limits
    await Task.Delay(1000);
}