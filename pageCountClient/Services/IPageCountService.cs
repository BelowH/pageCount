using pageCountClient.Domain;

namespace pageCountClient.Services;

public interface IPageCountService
{

    public void DisableAnalytics();

    public void EnableAnalytics();

    public Task<bool> SubmitCount(CountDto countDto);

    public Task<bool> SubmitCount(string primaryCountType, int amount = 1);

    public Task<bool> SubmitCount(string primaryCountType, string secondaryCountType, int amount = 1);



}