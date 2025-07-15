using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace BasicChatBot.Components.Pages;

/// <summary>
/// Code-behind class for the Error page component.
/// Handles error display and request tracking for debugging purposes.
/// </summary>
public partial class Error : ComponentBase
{
    #region Properties

    /// <summary>
    /// The current HTTP context provided by the cascading parameter
    /// </summary>
    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    /// <summary>
    /// The unique request identifier for tracking and debugging
    /// </summary>
    private string? RequestId { get; set; }

    /// <summary>
    /// Indicates whether the request ID should be displayed to the user
    /// </summary>
    private bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    #endregion

    #region Lifecycle Methods

    /// <summary>
    /// Initializes the error page by capturing the request ID from the current activity or HTTP context
    /// This helps with debugging and error tracking
    /// </summary>
    protected override void OnInitialized()
    {
        RequestId = Activity.Current?.Id ?? HttpContext?.TraceIdentifier;
    }

    #endregion
} 