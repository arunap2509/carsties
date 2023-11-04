using Auction.Controllers;
using Auction.Dto;
using Auction.RequestHelper;
using Auction.Services;
using Auction.UnitTests.Utils;
using AutoFixture;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Auction.UnitTests;

public class AuctionControllerTests
{
    private readonly Mock<IAuctionService> _auctionService;
    private readonly Mock<IPublishEndpoint> _publishEndpoint;
    private readonly Fixture _fixture;
    private readonly AuctionController _controller;
    private readonly IMapper _mapper;

    public AuctionControllerTests()
    {
        _fixture = new Fixture();
        _auctionService = new Mock<IAuctionService>();
        _publishEndpoint = new Mock<IPublishEndpoint>();

        var mockMapper = new MapperConfiguration(mc =>
        {
            mc.AddMaps(typeof(MappingProfiles).Assembly);
        }).CreateMapper().ConfigurationProvider;

        _mapper = new Mapper(mockMapper);
        _controller = new AuctionController(_auctionService.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = Helper.GetClaimsPrincipal() }
            }
        };
    }

    [Fact]
    public async Task GetAuctions_WithNoParams_Returns10Auctions()
    {
        // arrange
        var auctions = _fixture.CreateMany<AuctionDto>(10).ToList();
        _auctionService.Setup(repo => repo.GetAllAuctionsAsync(null)).ReturnsAsync(auctions);

        // act
        var result = await _controller.GetAllAuctions(null);

        var okResult = result as OkObjectResult;

        // assert
        Assert.NotNull(okResult);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetAuctionById_WithValidGuid_ReturnsAuction()
    {
        // arrange
        var auction = _fixture.Create<AuctionDto>();
        _auctionService.Setup(repo => repo.GetAuctionByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<AuctionDto> { auction });

        // act
        var result = await _controller.GetAuctionById(auction.Id);
        var okResult = result as OkObjectResult;

        // assert
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task CreateAuction_WithValidCreateAuctionDto_ReturnsCreatedAtAction()
    {
        // arrange
        var auction = _fixture.Create<CreateAuctionDto>();
        _auctionService.Setup(repo => repo.CreateAuctionAsync(It.IsAny<CreateAuctionDto>(), It.IsAny<string>()));

        // act
        var result = await _controller.CreateAuction(auction);
        var okResult = result as OkObjectResult;

        // assert
        Assert.NotNull(okResult);
    }
}
