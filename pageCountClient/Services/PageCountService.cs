using pageCountClient.Domain;
using pageCountClient.Repository;

namespace pageCountClient.Services;

internal class PageCountService : IPageCountService
{

    private PageCountOptions _options;
    private bool _analyticsEnabled;
    
    private readonly bool _throwOnError;
    private readonly string _userId = Guid.NewGuid().ToString();
    private readonly IPageCountRepository _repository;
    public PageCountService(PageCountOptions options, IPageCountRepository repository)
    {
        _options = options;
        _repository = repository;
        _analyticsEnabled = options.AnalyticsEnabledAsDefault;
        _throwOnError = options.ThrowOnError;
    }
    
    public void DisableAnalytics() => _analyticsEnabled = false;

    public void EnableAnalytics() => _analyticsEnabled = true;

    public async Task<bool> SubmitCount(CountDto countDto)
    {
        try
        {
            await _repository.SubmitCount(countDto);
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