using System;
using System.Text;

namespace backendSpark.API.Middleware;

public class BasicAuthenticationMiddleware
{

    private const string USERNAME = "admin";
    private const string PASSWORD = "password"; 
    private readonly RequestDelegate _next;

    public BasicAuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string authHeader = context.Request.Headers["Authorization"];

        if (string.IsNullOrWhiteSpace(authHeader))
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized access. Please provide valid credentials.");
            return;
        }

        var auth = authHeader.Split([' '])[1];

        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader));;   
        
        var username = credentials.Split(':')[0];
        var password = credentials.Split(':')[1];

        if (username == USERNAME && password == PASSWORD)
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized access. Please provide valid credentials.");
            return;
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