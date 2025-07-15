using BasicChatBot.Components;
using BasicChatBot.Services;

namespace BasicChatBot
{
    /// <summary>
    /// Main entry point for the Sarcastic AI Chatbot application.
    /// This Blazor Server application demonstrates local AI integration using Ollama and Semantic Kernel.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Application entry point that configures services and the HTTP request pipeline.
        /// </summary>
        /// <param name="args">Command-line arguments</param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Blazor components for both Server and WebAssembly modes
            // This enables interactive components that can run on the server or client
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            // Register our custom ChatService for dependency injection
            // This service handles all AI interactions through Ollama and Semantic Kernel
            builder.Services.AddScoped<ChatService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline based on environment
            if (app.Environment.IsDevelopment())
            {
                // Enable WebAssembly debugging in development
                app.UseWebAssemblyDebugging();
            }
            else
            {
                // Production configuration with error handling and security headers
                app.UseExceptionHandler("/Error");
                
                // HSTS (HTTP Strict Transport Security) for enhanced security
                // Consider adjusting the duration for production scenarios
                app.UseHsts();
            }

            // Redirect HTTP requests to HTTPS for security
            app.UseHttpsRedirection();

            // Enable anti-forgery token validation for form submissions
            app.UseAntiforgery();

            // Serve static files (CSS, JS, images) with optimization
            app.MapStaticAssets();
            
            // Configure Razor Components with interactive render modes
            // This allows components to be interactive on both server and client side
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            // Start the application
            app.Run();
        }
    }
}
