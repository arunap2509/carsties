using Auction.Dto;
using Auction.Services;
using Microsoft.AspNetCore.Mvc;

namespace Auction.Controllers;

[ApiController]
[Route("api/auction")]
public class AuctionController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    public AuctionController(IAuctionService auctionService)
    {
        _auctionService = auctionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuctions(string date)
    {
        var auctions = await _auctionService.GetAllAuctionsAsync(date);
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuctionById(Guid id)
    {
        var auction = await _auctionService.GetAuctionByIdAsync(id);
        return Ok(auction);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = await _auctionService.CreateAuctionAsync(createAuctionDto);
        return Ok(auction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuction(Guid id, [FromBody] UpdateAuctionDto updateAuctionDto)
    {
        await _auctionService.UpdateAuctionAsync(id, updateAuctionDto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id)
    {
        await _auctionService.DeleteAuctionAsync(id);
        return Ok();
    }
}
