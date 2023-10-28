using AutoMapper;
using BiddingService.Dto;
using BiddingService.Models;
using Contracts;

namespace BiddingService.RequestHelper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Bid, BidDto>();
        CreateMap<Bid, BidPlaced>();
    }
}
