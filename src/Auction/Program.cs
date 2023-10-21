using Auction.Consumers;
using Auction.Data;
using Auction.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AuctionDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
    .UseSnakeCaseNamingConvention();
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IAuctionService, AuctionService>();
builder.Services.AddMassTransit(option =>
{
    option.AddEntityFrameworkOutbox<AuctionDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(10);

        o.UsePostgres();
        o.UseBusOutbox();
    });
    option.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
    option.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

    option.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

DbInitializer.InitDb(app);

app.Run();
