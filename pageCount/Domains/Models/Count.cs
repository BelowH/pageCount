namespace pageCount.Domains.Models;

public class Count
{

    public string CountId { get; set; } = Guid.NewGuid().ToString();

    public int Amount { get; set; }

    public string? PrimaryCountType { get; set; }

    public string? SecondaryCountType { get; set; }

    public DateTime Timestamp { get; set; }
    
}