using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Ollama;
using Microsoft.SemanticKernel.ChatCompletion;
using OllamaSharp;

namespace BasicChatBot.Services;

/// <summary>
/// Service responsible for managing AI chat interactions using Ollama and Microsoft Semantic Kernel.
/// This service maintains conversation context throughout the session and provides a sarcastic AI personality.
/// </summary>
public class ChatService
{
    #region Private Fields

    /// <summary>
    /// The chat completion service provided by Semantic Kernel for Ollama integration.
    /// </summary>
    private readonly IChatCompletionService _chatService;
    
    /// <summary>
    /// Persistent chat history that maintains conversation context across multiple interactions.
    /// This enables the AI to reference previous messages and maintain conversational flow.
    /// </summary>
    private readonly ChatHistory _chatHistory;
    
    /// <summary>
    /// System prompt that defines the AI's personality and behavior.
    /// This prompt engineering creates the sarcastic but helpful assistant personality.
    /// </summary>
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

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the ChatService with Ollama configuration.
    /// </summary>
    /// <param name="configuration">Application configuration containing Ollama settings</param>
    public ChatService(IConfiguration configuration)
    {
        // Extract Ollama configuration from appsettings.json with sensible defaults
        var ollamaEndpoint = configuration.GetValue<string>("Ollama:Endpoint") ?? "http://localhost:11434";
        var modelId = configuration.GetValue<string>("Ollama:ModelId") ?? "llama3.1:8b";
        
        // Create Ollama API client using the modern approach
        // This connects to the local Ollama service running on the specified endpoint
        var ollamaClient = new OllamaApiClient(
            uriString: ollamaEndpoint,
            defaultModel: modelId);

        // Suppress experimental API warning for Ollama connector
        // The Ollama connector is currently in alpha but stable for this use case
#pragma warning disable SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
        _chatService = ollamaClient.AsChatCompletionService();
#pragma warning restore SKEXP0001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

        // Initialize persistent chat history with the system prompt
        // This ensures the AI maintains its personality throughout the conversation
        _chatHistory = new ChatHistory(_systemPrompt);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Processes a user message and returns an AI response while maintaining conversation context.
    /// This method adds the user message to the persistent history, gets an AI response,
    /// and adds the response back to the history for future context.
    /// </summary>
    /// <param name="userMessage">The message from the user to process</param>
    /// <returns>A task containing the AI's response as a string</returns>
    public async Task<string> GetResponseAsync(string userMessage)
    {
        try
        {
            // Add the user's message to our persistent conversation history
            // This ensures the AI has context of what the user just said
            _chatHistory.AddUserMessage(userMessage);

            // Send the entire conversation history to the AI model
            // This allows the AI to reference previous messages and maintain context
            var response = await _chatService.GetChatMessageContentAsync(_chatHistory);
            
            // Extract the response content with a fallback message that matches our personality
            var aiResponse = response.Content ?? "I seem to be speechless... how unusual! 😅";

            // Add the AI's response to the persistent history
            // This ensures future responses can reference what the AI previously said
            _chatHistory.AddAssistantMessage(aiResponse);

            return aiResponse;
        }
        catch (Exception ex)
        {
            // Create an error response that maintains the sarcastic personality
            var errorResponse = $"Well, this is awkward... I seem to have encountered an error: {ex.Message}. Maybe try asking me something easier? 😏";
            
            // Add the error response to history to maintain conversation flow
            // This prevents the conversation from breaking due to errors
            _chatHistory.AddAssistantMessage(errorResponse);
            
            return errorResponse;
        }
    }

    /// <summary>
    /// Clears the conversation history and resets the chat to a fresh state.
    /// This removes all previous messages while preserving the AI's personality through the system prompt.
    /// </summary>
    public void ClearHistory()
    {
        // Clear all messages from the conversation history
        _chatHistory.Clear();
        
        // Re-add the system prompt to maintain the AI's personality
        // This ensures the sarcastic assistant behavior is preserved even after clearing
        _chatHistory.AddSystemMessage(_systemPrompt);
    }

    #endregion
} 