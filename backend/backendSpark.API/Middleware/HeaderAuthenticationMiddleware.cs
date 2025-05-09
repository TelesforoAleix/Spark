using System;

namespace backendSpark.API.Middleware
{
    public class HeaderAuthenticationMiddleware
    {

        private const string SecretValue = "X-Auth-Token"; 
        private readonly RequestDelegate _next;

        public HeaderAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string authHeader = context.Request.Headers["X-My-Request-Header"];

            if (!string.IsNullOrWhiteSpace(authHeader))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized access. Please provide valid credentials.");
                return;
            }

            if (!string.Equals(authHeader, SecretValue))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Unauthorized access. Please provide valid credentials.");
                return;
            }

            await _next(context);
        }
    }

    public static class HeaderAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderAuthenticationMiddleware>();
        }
    }
}