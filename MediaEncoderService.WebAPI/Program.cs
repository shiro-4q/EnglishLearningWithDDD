using Microsoft.OpenApi;

using MediaEncoderService.Infrastructure.Persistence;
using MediaEncoderService.WebAPI.BgServices;
using Q.Initializer;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
var initializerOptions = new InitializerOptions
{
    SwaggerTitle = "MediaEncoderService.API V1"
};
builder.ConfigureExtraServices(initializerOptions);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.Configure<TranscodeBgServiceOptions>(builder.Configuration.GetSection("TranscodeBgService"));
builder.Services.AddSingleton(sp =>
{
    var connectionMultiplexer = sp.GetRequiredService<IConnectionMultiplexer>();
    return RedLockFactory.Create([new RedLockMultiplexer(connectionMultiplexer)]);
});
builder.Services.AddHostedService<TranscodeBgService>();

builder.Services.AddCap(x =>
{
    x.UseEntityFramework<TranscodingDbContext>();

    x.UseRabbitMQ(opt =>
    {
        opt.HostName = "localhost";
        opt.UserName = "rmquser";
        opt.Password = "rmqpassword";
        opt.Port = 5672;
        opt.ExchangeName = "MediaEncoderService";
    });

    x.FailedRetryCount = 5;
    x.FailedRetryInterval = 30;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseExtraMiddleware(initializerOptions);

app.MapControllers();

app.Run();
