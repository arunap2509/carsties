using AutoMapper;
using BiddingService.Dto;
using BiddingService.Enums;
using BiddingService.Models;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/bids")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcAuctionClient _grpcAuctionClient;

    public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcAuctionClient)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _grpcAuctionClient = grpcAuctionClient;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PlaceBid(string auctionId, int amount)
    {
        var auction = await DB.Find<Auction>().OneAsync(auctionId) ?? _grpcAuctionClient.GetAuction(auctionId);

        if (auction == null)
        {
            return BadRequest("Cannot accept bids for this auction at this time");
        }

        if (auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }

        var bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };

        if (auction.AuctionEnd < DateTime.UtcNow)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            var highBid = await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(a => a.Descending(x => x.Amount))
            .ExecuteFirstAsync();

            if (highBid != null && amount > highBid.Amount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice
                                ? BidStatus.Accepted : BidStatus.AcceptedBelowReserve;
            }

            if (highBid != null && bid.Amount <= auction.ReservePrice)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);

        await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));

        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<IActionResult> GetBidsForAuction(string auctionId)
    {
        var bids = await DB.Find<Bid>().Match(x => x.AuctionId == auctionId)
        .Sort(s => s.Descending(a => a.BidTime)).ExecuteAsync();

        return Ok(bids.Select(_mapper.Map<BidDto>).ToList());
    }
}
