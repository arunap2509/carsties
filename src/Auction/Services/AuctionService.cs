using Auction.Data;
using Auction.Dto;
using Auction.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Auction.Services;

public interface IAuctionService
{
    Task<List<AuctionDto>> GetAllAuctionsAsync(string date);
    Task<List<AuctionDto>> GetAuctionByIdAsync(Guid id);
    Task<AuctionDto> CreateAuctionAsync(CreateAuctionDto createAuctionDto, string username);
    Task UpdateAuctionAsync(Guid id, string username, UpdateAuctionDto updateAuctionDto);
    Task DeleteAuctionAsync(Guid id, string username);
}

public class AuctionService : IAuctionService
{
    private readonly AuctionDbContext _auctionDbContext;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionService(AuctionDbContext auctionDbContext, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _auctionDbContext = auctionDbContext;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<AuctionDto> CreateAuctionAsync(CreateAuctionDto createAuctionDto, string username)
    {
        var auction = _mapper.Map<Auctions>(createAuctionDto);
        auction.Seller = username;
        await _auctionDbContext.Auctions.AddAsync(auction);

        var auctionDto = _mapper.Map<AuctionDto>(auction);

        var auctionCreated = _mapper.Map<AuctionCreated>(auctionDto);
        await _publishEndpoint.Publish(auctionCreated);

        await _auctionDbContext.SaveChangesAsync();
        return auctionDto;
    }

    public async Task DeleteAuctionAsync(Guid id, string username)
    {
        var auction = await _auctionDbContext.Auctions.FindAsync(id);

        if (auction == null)
        {
            // return error response
        }

        if (auction.Seller != username)
        {
            // forbidden
        }

        _auctionDbContext.Auctions.Remove(auction);
        await _publishEndpoint.Publish(new AuctionDeleted { Id = id.ToString() });
        await _auctionDbContext.SaveChangesAsync();
    }

    public async Task<List<AuctionDto>> GetAllAuctionsAsync(string date)
    {
        var query = _auctionDbContext.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query.Where(x => x.AuctionEnd.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<List<AuctionDto>> GetAuctionByIdAsync(Guid id)
    {
        var auctions = _auctionDbContext.Auctions
                            .Include(x => x.Item)
                            .Where(x => x.Id == id)
                            .OrderBy(x => x.Item.Make)
                            .AsQueryable();

        return await auctions.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task UpdateAuctionAsync(Guid id, string username, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _auctionDbContext.Auctions.FindAsync(id);

        if (auction == null)
        {
            // not found
        }

        if (auction.Seller != username)
        {
            // forbidden
        }

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        _auctionDbContext.Auctions.Update(auction);

        var auctionUpdated = _mapper.Map<AuctionUpdated>(auction);

        await _publishEndpoint.Publish(auctionUpdated);
        await _auctionDbContext.SaveChangesAsync();
    }
}
