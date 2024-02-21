namespace pageCount.Domains.Models;

public class Count
{

    public Count()
    {
        
    }

    public Count(CountDto countDto)
    {
        Amount = countDto.Amount;
        UserId = countDto.UserId;
        PrimaryCountType = countDto.PrimaryCountType;
        SecondaryCountType = countDto.SecondaryCountType;
        Timestamp = countDto.Timestamp;
    }
    
    
    public string CountId { get; set; } = Guid.NewGuid().ToString();

    public string? UserId { get; set; }
    
    public int Amount { get; set; }

    public string? PrimaryCountType { get; set; }

    public string? SecondaryCountType { get; set; }

    public DateTime Timestamp { get; set; }
    
}