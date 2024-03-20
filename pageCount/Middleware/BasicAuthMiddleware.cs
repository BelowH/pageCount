using System.Security.Cryptography;
using System.Text;

namespace pageCount.Middleware;

public class BasicAuthMiddleware
{

    private readonly RequestDelegate _next;

    private readonly IConfiguration _configuration;

    public BasicAuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            string? authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string? encodedUsernamePassword =
                    authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                string decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword!));
                string username = decodedUsernamePassword.Split(':', 2)[0];
                string password = decodedUsernamePassword.Split(':', 2)[1];

                // Validate credentials
                if (!IsAuthorized(username, password))
                {
                    throw new Exception();
                }
                await _next(context);
            }
        }
        catch
        {
            // Not authorized
            context.Response.Headers["WWW-Authenticate"] = "Basic";
            context.Response.StatusCode = 401; // Unauthorized
            await context.Response.WriteAsync("Unauthorized"); 
        }
    }

    private bool IsAuthorized(string usernameChallenger, string passwordChallenger)
    {
        string? username = _configuration["usernameHash"];
        string? password = _configuration["passwordHash"];

        if (username is null || password is null )
        {
            return false;
        }

        string usernameChallengerHash = ToSha256(usernameChallenger);
        string passwordChallengerHash = ToSha256(passwordChallenger);

        return usernameChallengerHash.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
               passwordChallengerHash.Equals(password, StringComparison.InvariantCultureIgnoreCase);
    }


    private static string ToSha256(string plain)
    {
        StringBuilder sb = new StringBuilder();
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(plain));
        foreach (byte hashedByte in hashedBytes)
        {
            sb.Append(hashedByte.ToString("X2"));
        }
        return sb.ToString();
    }


}