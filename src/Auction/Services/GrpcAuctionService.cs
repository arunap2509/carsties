using Auction.Data;
using Grpc.Core;

namespace Auction.Services;

public class GrpcAuctionService : GrpcAuction.GrpcAuctionBase
{
    private readonly AuctionDbContext _auctionDbContext;

    public GrpcAuctionService(AuctionDbContext auctionDbContext)
    {
        _auctionDbContext = auctionDbContext;
    }

    public override async Task<GrpcAuctionResponse> GetAuction(GrpcAuctionRequest request, ServerCallContext context)
    {
        Console.WriteLine("received request for auction");

        var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(request.Id)) ?? throw new RpcException(new Status(StatusCode.NotFound, "Not found"));

        var response = new GrpcAuctionResponse
        {
            Auction = new GrpcAuctionModel
            {
                AuctionEnd = auction.AuctionEnd.ToString(),
                Id = auction.Id.ToString(),
                ReservePrice = auction.ReservePrice,
                Seller = auction.Seller
            }
        };

        return response;
    }
}
