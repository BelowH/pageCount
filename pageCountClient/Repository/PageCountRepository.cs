using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using pageCountClient.Domain;

namespace pageCountClient.Repository;

internal class PageCountRepository : IPageCountRepository
{

    private readonly HttpClient _client;
    
    
    public PageCountRepository(PageCountOptions options)
    {
        _client = new HttpClient();
        _client.BaseAddress = options.HostUri;
        string authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.Username}:{options.Password}"));
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authString);
    }

    public async Task SubmitCount(CountDto countDto)
    {
        string contentAsString = JsonSerializer.Serialize(countDto);
        HttpContent content = new StringContent(contentAsString);
        HttpResponseMessage response = await _client.PostAsync("count",content);
        response.EnsureSuccessStatusCode();
    }
}