# 🤖 Sarcastic AI Chatbot - Ollama + Semantic Kernel + .NET

A modern chatbot built with .NET 9, Blazor, Ollama, and Microsoft Semantic Kernel that delivers helpful responses with a delightfully sarcastic personality.

## 🎥 YouTube Channel

Follow along with AI development tutorials and .NET content:
**[www.youtube.com/@DotNetAIGuy](https://www.youtube.com/@DotNetAIGuy)**

## 🚀 Features

- **🦙 Local AI Processing**: Powered by Ollama running LLAMA 3.1 locally
- **🧠 Semantic Kernel Integration**: Microsoft's AI orchestration framework  
- **😏 Sarcastic Personality**: AI with attitude - helpful but cheeky responses
- **💭 Conversation Memory**: Maintains context throughout the chat session
- **💬 Modern Chat Interface**: Clean, responsive chat experience
- **📱 Cross-Platform**: Works on desktop and mobile devices

## 📋 Prerequisites

Before running this project, ensure you have:

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Ollama](https://ollama.ai/) installed and running
- LLAMA 3.1 model downloaded

## 🛠️ Ollama Setup

### 1. Install Ollama

Visit the official Ollama website for installation instructions for your platform:
**[https://ollama.ai/download](https://ollama.ai/download)**

### 2. Start Ollama Service

```bash
ollama serve
```

### 3. Download AI Models

**PowerShell/Bash Commands:**

```bash
# Download the 8B parameter model (recommended - good balance of performance and speed)
ollama pull llama3.1:8b

# Alternative models:
ollama pull llama3.1:7b     # Smaller, faster model for lower-end hardware
ollama pull llama3.1:70b    # Larger model for better responses (requires 40GB+ RAM)
ollama pull phi3:mini       # Microsoft's efficient small model
ollama pull mistral:7b      # Alternative high-quality model
```

### 4. Verify Installation

```bash
# List installed models
ollama list

# Test the model
ollama run llama3.1:8b
```

## 🧠 Semantic Kernel Overview

Microsoft Semantic Kernel is an open-source SDK that lets you easily combine AI services with conventional programming languages like C#.

### Key Benefits:
- **Abstraction**: Write once, run with any AI service
- **Memory**: Maintains conversation context
- **Orchestration**: Chain AI functions together  
- **Enterprise-Ready**: Built with security and scalability in mind

## 🏗️ Project Structure

```
BasicChatBot/
├── BasicChatBot/                 # Main Blazor Server project
│   ├── Components/Pages/
│   │   └── Home.razor           # Chat interface
│   ├── Services/
│   │   └── ChatService.cs       # Ollama + Semantic Kernel integration
│   └── Program.cs              # Application configuration
└── README.md
```

## 🔧 Installation & Running

### 1. Clone the Repository

```bash
git clone <repository-url>
cd BasicChatBot
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Model (Optional)

Edit `appsettings.json` to customize the AI model:

```json
{
  "Ollama": {
    "Endpoint": "http://localhost:11434",
    "ModelId": "llama3.1:8b"
  }
}
```

### 4. Run the Application

```bash
cd BasicChatBot/BasicChatBot
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

## 🎯 Prompt Engineering - Creating AI Personalities

This project demonstrates how to use prompt engineering to create distinct AI personalities. The sarcastic chatbot uses a carefully crafted system prompt:

```csharp
private readonly string _systemPrompt = """
    You are a cheeky, sarcastic AI assistant with a sharp wit and playful attitude. 
    Your responses should be:
    - Sarcastic but not mean-spirited
    - Clever and witty
    - Helpful despite the sarcasm
    - Confident and a bit cheeky
    - Use humor to make conversations engaging
    
    Always maintain a friendly tone underneath the sarcasm. 
    You're like that friend who teases but ultimately cares.
    Keep responses concise and punchy when possible.
    """;
```

### Key Prompt Engineering Principles:
1. **Clear Personality Definition**: Define character traits clearly
2. **Behavioral Guidelines**: Specify how the AI should behave  
3. **Tone and Style**: Set expectations for communication style
4. **Boundaries**: Establish what's acceptable and what isn't

## 🔌 How It Works

### Semantic Kernel with Ollama Integration

The application uses the latest Semantic Kernel syntax for Ollama:

```csharp
// Modern OllamaApiClient approach
var ollamaClient = new OllamaApiClient(
    uriString: "http://localhost:11434",
    defaultModel: "llama3.1:8b");

var chatService = ollamaClient.AsChatCompletionService();

// Persistent conversation history
var chatHistory = new ChatHistory(systemPrompt);
chatHistory.AddUserMessage(userMessage);

var response = await chatService.GetChatMessageContentAsync(chatHistory);
```

### Conversation Memory

The application maintains conversation context throughout the session:
- **Persistent ChatHistory**: Remembers all previous messages
- **Context Awareness**: AI can reference earlier topics
- **Clear Chat**: Option to reset conversation and start fresh

## 🔍 Troubleshooting

### Common Issues:

**1. Model Not Found Error**
```bash
# Download the model first
ollama pull llama3.1:8b
```

**2. Ollama Not Running**
```bash
# Start the Ollama service
ollama serve
```

**3. Slow Responses**
- Use a smaller model: `ollama pull llama3.1:7b` or `ollama pull phi3:mini`
- Ensure sufficient RAM is available (8GB+ recommended)
- Close other applications to free up resources

**4. Connection Issues**
- Verify Ollama is running on port 11434
- Check firewall settings
- Try restarting the Ollama service

**5. Out of Memory Errors**
- Use smaller models (7B parameters or less)
- Increase system RAM or swap space
- Restart Ollama service

## 🚀 Extensions & Next Steps

Ideas for extending this project:
1. **Multiple AI Models**: Switch between different models
2. **Export Conversations**: Save chat history
3. **Custom Personalities**: Create different AI personas
4. **Plugin System**: Add Semantic Kernel plugins for web search, calculations
5. **Voice Integration**: Add speech-to-text and text-to-speech

## 📚 Learning Resources

### Ollama
- [Official Documentation](https://ollama.ai/docs)
- [Model Library](https://ollama.ai/library) 
- [GitHub Repository](https://github.com/ollama/ollama)

### Semantic Kernel
- [Official Documentation](https://learn.microsoft.com/en-us/semantic-kernel/)
- [GitHub Repository](https://github.com/microsoft/semantic-kernel)
- [Chat Completion Guide](https://learn.microsoft.com/en-us/semantic-kernel/concepts/ai-services/chat-completion/)

### .NET & Blazor
- [.NET 9 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Blazor Documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/)

## 🤝 Contributing

Feel free to submit issues, fork the repository, and create pull requests for any improvements.

## 📄 License

This project is licensed under the MIT License.

## 🎬 Follow for More Content

Subscribe to **[www.youtube.com/@DotNetAIGuy](https://www.youtube.com/@DotNetAIGuy)** for more AI development tutorials with .NET!

---

**Happy Coding! 🚀**

*Built with ❤️ using .NET 9, Blazor, Ollama, and Semantic Kernel* 