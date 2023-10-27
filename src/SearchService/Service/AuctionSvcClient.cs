using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Service;

public class AuctionSvcClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    public AuctionSvcClient(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetAuctionByDate()
    {
        var lastUpdated = DB.Find<Item, string>()
                            .Sort(x => x.Descending(y => y.UpdatedAt))
                            .Project(x => x.UpdatedAt.ToString())
                            .ExecuteFirstAsync();

        return await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date" + lastUpdated.ToString());
    }
}
