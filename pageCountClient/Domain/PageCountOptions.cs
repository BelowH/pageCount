namespace pageCountClient.Domain;

public class PageCountOptions
{

    public Uri? HostUri { get; init; }

    public string? Username { get; init; }

    public string? Password { get; init; }

    public bool AnalyticsEnabledAsDefault { get; init; }
    
    public bool ThrowOnError { get; init; }
    
}