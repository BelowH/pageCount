using pageCount.Domains;
using pageCount.Domains.Models;

namespace pageCount;

public interface ICountService
{

    public Task<bool> SubmitCount(CountDto countDto);

}

public class CountService : ICountService
{

    private readonly IDatabaseRepository _databaseRepository;
    
    public CountService(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }
    
    
    public async Task<bool> SubmitCount(CountDto countDto)
    {
        Count count = new Count(countDto);
        bool insertSuccessful = await _databaseRepository.AddCount(count);
        return insertSuccessful;
    }
}