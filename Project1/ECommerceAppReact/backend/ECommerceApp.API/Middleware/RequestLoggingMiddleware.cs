using System.Diagnostics;
using System.Text;

namespace ECommerceApp.API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _logFilePath;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
        // Log file in the root of the API project
        _logFilePath = Path.Combine(Directory.GetCurrentDirectory(), "api_log.txt");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString("N")[..8];
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        
        // --- 1. Capture Request ---
        var method = context.Request.Method;
        var path = context.Request.Path;
        var query = context.Request.QueryString;
        
        var sb = new StringBuilder();
        sb.AppendLine($"[{timestamp}] [{requestId}] ➡️  REQUEST: {method} {path}{query}");
        
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
             var token = context.Request.Headers["Authorization"].ToString();
             // Show first 50 chars to distinguish between "Bearer mock-..." and real JWT "Bearer eyJ..."
             var maskedToken = token.Length > 50 ? token.Substring(0, 50) + "..." : token;
             sb.AppendLine($"    Auth: {maskedToken}");
        }

        await LogToFileAsync(sb.ToString().TrimEnd());

        // --- 2. Capture Response Body ---
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        try
        {
            await _next(context);

            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;
            var statusEmoji = statusCode < 400 ? "✅" : statusCode < 500 ? "⚠️" : "❌";
            
            // Read response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            var logMessage = new StringBuilder();
            logMessage.AppendLine($"[{timestamp}] [{requestId}] {statusEmoji} RESPONSE: {statusCode} ({stopwatch.ElapsedMilliseconds}ms)");
            
            if (!string.IsNullOrEmpty(responseText))
            {
                // Truncate long responses but keep enough for debugging
                var preview = responseText.Length > 1000 ? responseText.Substring(0, 1000) + "... [truncated]" : responseText;
                logMessage.AppendLine($"    Body: {preview}");
            }
            
            await LogToFileAsync(logMessage.ToString());

            await responseBody.CopyToAsync(originalBodyStream);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            var errorMessage = new StringBuilder();
            errorMessage.AppendLine($"[{timestamp}] [{requestId}] ❌ EXCEPTION ({stopwatch.ElapsedMilliseconds}ms):");
            errorMessage.AppendLine($"    Message: {ex.Message}");
            errorMessage.AppendLine($"    Stack: {ex.StackTrace}");
            
            await LogToFileAsync(errorMessage.ToString());
            
            // Re-throw so standard error handling works
            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
            throw;
        }
    }

    private async Task LogToFileAsync(string message)
    {
        try 
        {
            // Simple file lock to avoid concurrency issues (quick & dirty for dev)
            lock (_logFilePath)
            {
                File.AppendAllText(_logFilePath, message + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);
            }
            await Task.CompletedTask;
        }
        catch 
        {
            // Ignore logging errors to not crash app
        }
    }
}

public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestLoggingMiddleware>();
    }
}
