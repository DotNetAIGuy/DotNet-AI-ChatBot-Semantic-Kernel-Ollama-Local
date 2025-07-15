using BasicChatBot.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BasicChatBot.Components.Pages;

/// <summary>
/// Code-behind class for the Home page component.
/// Handles all chat functionality including message management, AI interactions, and UI state.
/// </summary>
public partial class Home : ComponentBase
{
    #region Private Fields

    /// <summary>
    /// Collection of chat messages for display in the UI
    /// </summary>
    private List<ChatMessage> messages = new();

    /// <summary>
    /// Current message being typed by the user
    /// </summary>
    private string currentMessage = "";

    /// <summary>
    /// Indicates whether the AI is currently processing a request
    /// </summary>
    private bool isLoading = false;

    /// <summary>
    /// Reference to the messages container for scrolling functionality
    /// </summary>
    private ElementReference messagesContainer;

    /// <summary>
    /// Reference to the message input field for focus management
    /// </summary>
    private ElementReference messageInput;

    #endregion

    #region Injected Services

    /// <summary>
    /// Injected ChatService for AI interactions
    /// </summary>
    [Inject]
    private ChatService ChatService { get; set; } = default!;

    /// <summary>
    /// Injected JavaScript runtime for DOM operations
    /// </summary>
    [Inject]
    private IJSRuntime JS { get; set; } = default!;

    #endregion

    #region Data Models

    /// <summary>
    /// Represents a single chat message in the conversation
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// The text content of the message
        /// </summary>
        public string Content { get; set; } = "";

        /// <summary>
        /// Indicates whether this message was sent by the user (true) or AI (false)
        /// </summary>
        public bool IsUser { get; set; }

        /// <summary>
        /// Timestamp when the message was created
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handles keyboard input in the message field, specifically Enter key for sending messages
    /// </summary>
    /// <param name="e">Keyboard event arguments</param>
    private async Task HandleKeyPress(KeyboardEventArgs e)
    {
        // Send message when Enter is pressed (without Shift for new lines)
        if (e.Key == "Enter" && !e.ShiftKey)
        {
            await SendMessage();
        }
    }

    /// <summary>
    /// Processes sending a user message and getting an AI response
    /// This method handles the complete flow: validation, UI updates, API call, and error handling
    /// </summary>
    private async Task SendMessage()
    {
        // Validate input and loading state
        if (string.IsNullOrWhiteSpace(currentMessage) || isLoading)
            return;

        // Capture and clear the current message
        var userMessage = currentMessage.Trim();
        currentMessage = "";

        // Add user message to the UI immediately for responsive feel
        messages.Add(new ChatMessage { Content = userMessage, IsUser = true });
        isLoading = true;
        StateHasChanged();

        // Scroll to show the new message
        await ScrollToBottom();

        try
        {
            // Get AI response from the ChatService
            var response = await ChatService.GetResponseAsync(userMessage);
            
            // Add AI response to the conversation
            messages.Add(new ChatMessage { Content = response, IsUser = false });
        }
        catch (Exception ex)
        {
            // Display user-friendly error message if something goes wrong
            messages.Add(new ChatMessage 
            { 
                Content = $"Oops! Something went wrong: {ex.Message}", 
                IsUser = false 
            });
        }
        finally
        {
            // Reset loading state and update UI
            isLoading = false;
            StateHasChanged();
            await ScrollToBottom();
            await FocusInput();
        }
    }

    /// <summary>
    /// Clears all messages from the UI and resets the AI's conversation history
    /// </summary>
    private async Task ClearChat()
    {
        // Clear UI messages
        messages.Clear();
        
        // Clear the AI's conversation memory
        ChatService.ClearHistory();
        
        // Update UI and return focus to input
        StateHasChanged();
        await FocusInput();
    }

    #endregion

    #region UI Helper Methods

    /// <summary>
    /// Scrolls the message container to the bottom to show the latest message
    /// Includes a small delay to ensure DOM updates are complete
    /// </summary>
    private async Task ScrollToBottom()
    {
        await Task.Delay(100); // Small delay to ensure DOM is updated
        await JS.InvokeVoidAsync("scrollToBottom", messagesContainer);
    }

    /// <summary>
    /// Returns focus to the message input field for seamless user experience
    /// Includes a small delay to ensure DOM updates are complete
    /// </summary>
    private async Task FocusInput()
    {
        await Task.Delay(100); // Small delay to ensure DOM is updated
        await messageInput.FocusAsync();
    }

    #endregion
} 