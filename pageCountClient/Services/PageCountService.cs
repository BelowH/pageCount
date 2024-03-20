using pageCountClient.Domain;
using pageCountClient.Repository;

namespace pageCountClient.Services;

internal class PageCountService(PageCountOptions options, IPageCountRepository repository) : IPageCountService
{
    private PageCountOptions _options = options;
    private bool _analyticsEnabled = options.AnalyticsEnabledAsDefault;
    
    private readonly bool _throwOnError = options.ThrowOnError;
    private readonly string _userId = Guid.NewGuid().ToString();

    public void DisableAnalytics() => _analyticsEnabled = false;

    public void EnableAnalytics() => _analyticsEnabled = true;

    public async Task<bool> SubmitCount(CountDto countDto)
    {
        try
        {
            await repository.SubmitCount(countDto);
            return true;
        }
        catch (Exception e)
        {
            if (_throwOnError)
            {
                throw new SubmitCountException($"An Exception occured while submitting a count, see innerException for more details", e);
            }
            return false;
        }
    }

    public Task<bool> SubmitCount(string primaryCountType, int amount = 1)
    {
        CountDto countDto = new CountDto()
        {
            UserId = _userId,
            PrimaryCountType = primaryCountType,
            Amount = amount,
        };
        return SubmitCount(countDto);
    }

    public Task<bool> SubmitCount(string primaryCountType, string secondaryCountType, int amount = 1)
    {
        CountDto countDto = new CountDto()
        {
            UserId = _userId,
            PrimaryCountType = primaryCountType,
            SecondaryCountType = secondaryCountType,
            Amount = amount
        };
        return SubmitCount(countDto);
    }
}