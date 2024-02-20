using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pageCount.Domains;

public class CountDto
{
    [JsonPropertyName("userId")]
    public string UserId = Guid.NewGuid().ToString();
    
    [JsonPropertyName("amount")]
    public int Amount { get; set; } = 1;
    
    [Required]
    [JsonPropertyName("primaryCountType")]
    public string PrimaryCountType { get; set; } = "";

    [JsonPropertyName("secondaryCountType")]
    public string SecondaryCountType { get; set; } = "";

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.Now;

}