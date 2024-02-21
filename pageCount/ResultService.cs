using pageCount.Domains.Models;

namespace pageCount;

public interface IResultService
{

    public Task<IEnumerable<Count>> GetCountsByIdentifier(string primaryIdentifier, string secondaryIdentifier = "");

}

public class ResultService : IResultService
{

    private readonly IDatabaseRepository _databaseRepository;

    public ResultService(IDatabaseRepository databaseRepository)
    {
        _databaseRepository = databaseRepository;
    }


    public async Task<IEnumerable<Count>> GetCountsByIdentifier(string primaryIdentifier, string secondaryIdentifier = "")
    {
        IEnumerable<Count> results = await _databaseRepository.GetByCountType(primaryIdentifier, secondaryIdentifier) ?? new List<Count>();
        return results;
    }
}