using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Service;

public class AuctionSvcClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<AuctionSvcClient> _logger;
    public AuctionSvcClient(HttpClient httpClient, IConfiguration config, ILogger<AuctionSvcClient> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<List<Item>> GetAuctionByDate()
    {
        var lastUpdated = await DB.Find<Item, string>()
                            .Sort(x => x.Descending(y => y.UpdatedAt))
                            .Project(x => x.UpdatedAt.ToString())
                            .ExecuteFirstAsync();

        if (lastUpdated != null)
        {
            lastUpdated = lastUpdated.ToString();
        }

        var url = _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;

        var data = await _httpClient.GetFromJsonAsync<List<Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated);

        return data;
    }
}
