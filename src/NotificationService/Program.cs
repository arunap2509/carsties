using MassTransit;
using NotificationService.Consumers;
using NotificationService.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddMassTransit(option =>
{
    option.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    option.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("nt", false));

    option.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMq:Host"], "/", host =>
        {
            host.Username(builder.Configuration.GetValue("RabbitMq:Username", "guest"));
            host.Password(builder.Configuration.GetValue("RabbitMq:Password", "guest"));
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapHub<NotificationHub>("/notifications");

app.Run();
