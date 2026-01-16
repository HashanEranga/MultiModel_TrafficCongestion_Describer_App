# Traffic Congestion Analyzer

A C# console application that uses Azure OpenAI's GPT-4o-mini model with vision capabilities to analyze traffic camera images and determine congestion levels while detecting camera malfunctions.

## Overview

This application processes traffic camera images to:
- **Analyze traffic congestion levels** (Light, Moderate, Heavy)
- **Detect camera malfunctions** by identifying distorted or corrupted images
- **Provide detailed analysis** of traffic conditions

The application leverages Microsoft Semantic Kernel and Azure OpenAI's multimodal capabilities to perform intelligent image analysis.

## Features

- **Automated Image Processing**: Batch processes all JPG images in the Images directory
- **AI-Powered Analysis**: Uses GPT-4o-mini vision model for accurate traffic assessment
- **Camera Health Monitoring**: Detects malfunctioning cameras through image distortion analysis
- **Structured Output**: Returns JSON-formatted results with congestion levels and analysis
- **Color-Coded Console Output**: Visual feedback with red for broken cameras, green for functioning ones
- **Rate Limiting**: Built-in delays to prevent API throttling

## Traffic Congestion Levels

The application classifies traffic into three categories:

- **Heavy Congestion**: Very little room between vehicles, cars are braking
- **Moderate Congestion**: Many cars present but traffic is flowing without braking
- **Light Traffic**: Few cars on the road

## Prerequisites

- **.NET 9.0 SDK** or later
- **Azure OpenAI Service** account with:
  - GPT-4o-mini model deployment
  - Valid API key and endpoint
- **Visual Studio 2022** or any C# IDE (optional)

## Installation

1. Clone or download this repository
2. Navigate to the project directory
3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

## Configuration

### appsettings.json

Update the `appsettings.json` file with your Azure OpenAI credentials:

```json
{
  "AzureOpenAI": {
    "ModelId": "gpt-4o-mini",
    "Endpoint": "https://your-resource-name.cognitiveservices.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "your-deployment-name"
  }
}
```

**Configuration Parameters:**
- `ModelId`: The model identifier (gpt-4o-mini)
- `Endpoint`: Your Azure OpenAI service endpoint URL
- `ApiKey`: Your Azure OpenAI API key
- `DeploymentName`: The name of your model deployment

⚠️ **Security Note**: Never commit your API keys to version control. Consider using environment variables or Azure Key Vault for production deployments.

## Usage

### Running the Application

1. Place traffic camera images (JPG format) in the `Images` directory
2. Run the application:
   ```bash
   dotnet run
   ```

### Sample Output

```
Processing image: Images/image1.jpg
Is Broken : False
Traffic Congestion Level : Heavy
Analysis : The image shows heavy traffic congestion with vehicles closely packed and brake lights visible...
----------------------------------------
Processing image: Images/image2.jpg
Is Broken : True
Traffic Congestion Level : Unknown
Analysis : The camera appears to be malfunctioning with significant image distortion...
----------------------------------------
```

## Project Structure

```
TrafficCongesionApp/
├── Program.cs                      # Main application entry point
├── CameraResult.cs                 # Data model for analysis results
├── TrafficCongestionLevel.cs       # Enum for congestion levels
├── appsettings.json                # Configuration file
├── TrafficCongesionApp.csproj      # Project file
├── Images/                         # Directory for traffic camera images
│   ├── image1.jpg
│   ├── image2.jpg
│   └── ...
└── README.md                       # This file
```

## Key Components

### CameraResult Class

Represents the analysis output:
```csharp
public class CameraResult
{
    public bool IsBroken { get; set; }
    public required string TrafficCongesionLevel { get; set; }
    public required string Analysis { get; set; }
}
```

### TrafficCongestionLevel Enum

Defines possible congestion states:
```csharp
public enum TrafficCongestionLevel
{
    Light,
    Moderate,
    Heavy,
    Unknown
}
```

## Dependencies

- **Microsoft.SemanticKernel** (v1.68.0): Framework for AI orchestration
- **Microsoft.SemanticKernel.Connectors.OpenAI** (v1.68.0): Azure OpenAI integration
- **Microsoft.Extensions.Configuration** (v10.0.1): Configuration management
- **Microsoft.Extensions.Configuration.Json** (v10.0.1): JSON configuration support

## How It Works

1. **Configuration Loading**: Reads Azure OpenAI settings from `appsettings.json`
2. **Service Initialization**: Creates an Azure OpenAI chat completion service
3. **Image Discovery**: Scans the Images directory for JPG files
4. **Image Processing**: For each image:
   - Loads the image as a byte array
   - Creates a chat history with system prompt defining analysis criteria
   - Sends the image to GPT-4o-mini for analysis
   - Receives structured JSON response
5. **Result Display**: Outputs color-coded results to console
6. **Rate Limiting**: Waits 1 second between requests

## System Prompt

The AI is instructed with the following criteria:

> "You are a traffic analyzer AI that monitors traffic congestion images and congestion level. Heavy congestion level is when there is very little room between cars and vehicles are braking. Medium congestion is when there is a lot of cars but they are not braking. Low traffic is when there are few cars on the road. In addition, attempt to determine if the image was taken with a malfunctioning camera by looking at distorted images."

## Customization

### Adjusting Rate Limits

Modify the delay in `Program.cs`:
```csharp
await Task.Delay(1000); // Change to desired milliseconds
```

### Changing Image Format

Update the file filter in `Program.cs`:
```csharp
var imageFiles = Directory.GetFiles("Images", "*.png"); // For PNG files
```

### Modifying Analysis Criteria

Edit the system message in the `ChatHistory` initialization to adjust how the AI evaluates traffic conditions.

## Troubleshooting

### Common Issues

**Issue**: "AzureOpenAI:Endpoint is not configured"
- **Solution**: Ensure `appsettings.json` contains valid Azure OpenAI configuration

**Issue**: No images processed
- **Solution**: Verify JPG images exist in the Images directory

**Issue**: API rate limit errors
- **Solution**: Increase the delay between requests or check your Azure OpenAI quota

**Issue**: Invalid API key errors
- **Solution**: Verify your API key is correct and has not expired

## Future Enhancements

Potential improvements for this application:
- Support for video stream analysis
- Real-time camera feed integration
- Database storage for historical analysis
- Web dashboard for visualization
- Alert system for severe congestion or camera failures
- Multi-camera comparison and correlation
- Export results to CSV or database

## License

This project is provided as-is for educational and demonstration purposes.

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## Contact

For questions or support, please open an issue in the repository.

---

**Note**: This application requires an active Azure OpenAI subscription. API usage will incur costs based on your Azure pricing plan.
