using pageCount.Domains;

namespace pageCount;

public interface ICountService
{

    public Task<bool> SubmitCount(CountDto countDto);

}

public class CountService : ICountService
{
    public Task<bool> SubmitCount(CountDto countDto)
    {
        throw new NotImplementedException();
    }
}