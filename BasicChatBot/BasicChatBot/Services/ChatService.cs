using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

namespace BasicChatBot.Services;

public class ChatService
{
    private readonly IChatCompletionService _chatService;
    private readonly ChatHistory _chatHistory;
    private readonly string _systemPrompt = """
        You are a cheeky, sarcastic AI assistant with a sharp wit and playful attitude. 
        Your responses should be:
        - Sarcastic but not mean-spirited
        - Clever and witty
        - Helpful despite the sarcasm
        - Confident and a bit cheeky
        - Use humor to make conversations engaging
        
        Always maintain a friendly tone underneath the sarcasm. You're like that friend who teases but ultimately cares.
        Keep responses concise and punchy when possible.
        """;

    public ChatService(IConfiguration configuration)
    {
        // Configure Ollama connection
        var ollamaEndpoint = configuration.GetValue<string>("Ollama:Endpoint") ?? "http://localhost:11434";
        var modelId = configuration.GetValue<string>("Ollama:ModelId") ?? "llama3.1:8b";
        
        var ollamaClient = new OllamaApiClient(
            uriString: ollamaEndpoint,
            defaultModel: modelId);

#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        _chatService = ollamaClient.AsChatCompletionService();
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Initialize persistent chat history with system prompt
        _chatHistory = new ChatHistory(_systemPrompt);
    }

    public async Task<string> GetResponseAsync(string userMessage)
    {
        try
        {
            // Add user message to persistent history
            _chatHistory.AddUserMessage(userMessage);

            // Get AI response using the full conversation history
            var response = await _chatService.GetChatMessageContentAsync(_chatHistory);
            var aiResponse = response.Content ?? "I seem to be speechless... how unusual! 😅";

            // Add AI response to persistent history
            _chatHistory.AddAssistantMessage(aiResponse);

            return aiResponse;
        }
        catch (Exception ex)
        {
            var errorResponse = $"Well, this is awkward... I seem to have encountered an error: {ex.Message}. Maybe try asking me something easier? 😏";
            
            // Add error response to history to maintain conversation flow
            _chatHistory.AddAssistantMessage(errorResponse);
            
            return errorResponse;
        }
    }

    public void ClearHistory()
    {
        _chatHistory.Clear();
        _chatHistory.AddSystemMessage(_systemPrompt);
    }
} 