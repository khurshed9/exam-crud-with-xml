namespace examdb.DelayMiddleware;

public class CustomMiddleware
{
    private readonly RequestDelegate _next;

    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var requestTime = DateTime.UtcNow;
        
        var clientIp = context.Connection.RemoteIpAddress?.ToString();
        
        var requestSize = context.Request.ContentLength ?? 0;

        
        Console.WriteLine($"Обработка запроса: {context.Request.Method} {context.Request.Path} " +
                          $"время: {requestTime:O}, IP: {clientIp}, размер: {requestSize} байт");

        
        await _next(context);

       
        var responseStatusCode = context.Response.StatusCode;
        Console.WriteLine($"Исходящий Ответ: {responseStatusCode} для {context.Request.Method} {context.Request.Path}");
    }
}