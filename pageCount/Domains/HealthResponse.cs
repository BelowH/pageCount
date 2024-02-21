using System.Reflection;
using System.Text.Json.Serialization;

namespace pageCount.Domains;

public class HealthResponse
{

    public HealthResponse(bool dbConnectionSuccessful)
    {
        if (dbConnectionSuccessful)
        {
            DatabaseConnection = "CONNECTED";
        }
        else
        {
            DatabaseConnection = "NOT CONNECTED";
        }
        
        
    }
    
    [JsonPropertyName("version")]
    public string Version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

    [JsonPropertyName("databaseConnection")]
    public string DatabaseConnection { get; set; }
    
}