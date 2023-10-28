using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Data;
using Contracts;
using MassTransit;

namespace Auction.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _auctionDbContext;

    public BidPlacedConsumer(AuctionDbContext auctionDbContext)
    {
        _auctionDbContext = auctionDbContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        var auction = await _auctionDbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

        if (auction.CurrentHighBid != null
        || context.Message.BidStatus.Contains("Accepted")
        && context.Message.Amount > auction.CurrentHighBid)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await _auctionDbContext.SaveChangesAsync();
        }
    }
}
