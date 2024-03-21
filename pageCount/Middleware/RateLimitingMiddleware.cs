using System.Net;

namespace pageCount.Middleware;

public class RateLimitingMiddleware
{

    private readonly RequestDelegate _next;
    private static Dictionary<string, (DateTime startTime, int limit)> _requestTimestamps = new Dictionary<string, (DateTime startTime, int limit)>();
    private int _limit;
    private TimeSpan _resetTime;
    
    public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;

        _limit = int.TryParse(configuration["rateLimit"], out int requestsPerMinute)
            ? requestsPerMinute
            : 100;
        _resetTime = TimeSpan.FromSeconds(60);

    }

    public async Task Invoke(HttpContext context)
    {
        
        string requestKey = $"{context.Request.Path}-{context.Connection.RemoteIpAddress}";
        lock (_requestTimestamps)
        {
            
            if (!_requestTimestamps.TryGetValue(requestKey, out (DateTime startTime, int count ) requestEntry))
            {
                _requestTimestamps.Add(requestKey, (DateTime.UtcNow, 1));
               
            }
            else
            {
                if (DateTime.UtcNow - requestEntry.startTime < _resetTime)
                {
                    if (requestEntry.count >= _limit)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                        context.Response.WriteAsync("Rate limit exceeded. Try again later.").RunSynchronously();
                        return;
                    }
                    else
                    {
                        _requestTimestamps[requestKey] = (requestEntry.startTime, requestEntry.count++);
                    }
                }
                else
                {
                    _requestTimestamps[requestKey] = (DateTime.UtcNow,1);
                }
            }
            
        }
        
        await _next(context);

    }
    
} 