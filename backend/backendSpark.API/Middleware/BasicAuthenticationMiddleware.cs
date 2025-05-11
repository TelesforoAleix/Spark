using System;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using backendSpark.Model.Repositories;

namespace backendSpark.API.Middleware;

public class BasicAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    

    public BasicAuthenticationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for login endpoint
        if (context.Request.Path.StartsWithSegments("/api/login", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        string authHeader = context.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized access. Please provide valid credentials.");
            return;
        }

        try
        {
            // Basic authentication header should be in format: "Basic base64string"
            if (!authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid authentication scheme. Use Basic Authentication.");
                return;
            }

            // Extract the base64 part (after "Basic ")
            var encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            
            // Decode the base64 string
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
            
            // Split into username:password
            var credentialParts = decodedCredentials.Split(':', 2);
            
            if (credentialParts.Length != 2)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid credentials format.");
                return;
            }

            var username = credentialParts[0];
            var password = credentialParts[1];

            // Create a scope to resolve the scoped service
            using (var scope = _scopeFactory.CreateScope())
            {
                // Get the UserRepository from the scope
                var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

                // Use the repository to check credentials
                var user = userRepository.GetUser(username, password);

                if (user != null)
                {
                    // Authentication successful
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Invalid credentials.");
                }
            }
        }
        catch (FormatException ex)
        {
            // Handle base64 decoding errors
            Console.WriteLine($"Authentication error: {ex.Message}");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid authorization header format.");
        }
        catch (Exception ex)
        {
            // Other unexpected errors
            Console.WriteLine($"Authentication error: {ex.Message}");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("An error occurred during authentication.");
        }
    }
}

public static class BasicAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseBasicAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BasicAuthenticationMiddleware>();
    }
}