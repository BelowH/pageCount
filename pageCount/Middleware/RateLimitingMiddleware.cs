using System.Net;

namespace pageCount.Middleware;

public class RateLimitingMiddleware
{

    private readonly RequestDelegate _next;
    private static Dictionary<string, DateTime> _requestTimestamps = new Dictionary<string, DateTime>();
    private int _requestsPerMinute;
    private TimeSpan _resetTime =  TimeSpan.FromSeconds(60);
    
    public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;

        _requestsPerMinute = int.TryParse(configuration["rateLimit"], out int requestsPerMinute)
            ? requestsPerMinute
            : 100;
    }

    public async Task Invoke(HttpContext context)
    {
        string requestKey = $"{context.Request.Path}-{context.Connection.RemoteIpAddress}";
        if (!_requestTimestamps.TryGetValue(requestKey, out DateTime requestTimestamp))
        {
            _requestTimestamps.Add(requestKey, DateTime.UtcNow);
            await _next(context);
        }
        else
        {
            if (DateTime.UtcNow - _requestTimestamps[requestKey] < _resetTime)
            {
                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
            }
            else
            {
                _requestTimestamps[requestKey] = DateTime.UtcNow;
                await _next(context);
            }
        }
    }
    
} 