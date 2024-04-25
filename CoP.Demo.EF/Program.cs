using System.Reflection;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using CoP.Demo.EF;
using CoP.Demo.EF.Domain;
using CoP.Demo.EF.Features;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services
    .AddDbContext<ApplicationDbContext>(x => x
        .UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumers(Assembly.GetExecutingAssembly());

    busConfig.AddEntityFrameworkOutbox<ApplicationDbContext>(cfg =>
    {
        cfg.UsePostgres();
        cfg.UseBusOutbox();
    });
    
    busConfig.UsingAmazonSqs((context, cfg) =>
    {
        cfg.Host("us-central-1", h =>
        {
            h.AccessKey("admin");
            h.SecretKey("admin");

            h.Config(new AmazonSQSConfig { ServiceURL = "http://localhost:4566" });
            h.Config(new AmazonSimpleNotificationServiceConfig { ServiceURL = "http://localhost:4566" });
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/orders", async (ISender sender) =>
    {
        var result = await sender.Send(new CreateOrderCommand());
        return result;
    })
    .WithName("CreateOrder")
    .WithOpenApi();

app.MapPatch("/orders", async (ISender sender, Guid id, Status status) =>
    {
        await sender.Send(new UpdateOrderCommand(id, status));
    })
    .WithName("UpdateOrder")
    .WithOpenApi();

app.Run();