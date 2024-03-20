using pageCountClient.Domain;

namespace pageCountClient.Repository;

internal interface IPageCountRepository
{

    public Task SubmitCount(CountDto countDto);
    

}