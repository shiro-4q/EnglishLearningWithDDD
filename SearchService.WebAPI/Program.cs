using Elastic.Clients.Elasticsearch;
using Q.Initializer;
using SearchService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var initializerOptions = new InitializerOptions
{
    SwaggerTitle = "SearchService.API V1"
};
builder.ConfigureExtraServices(initializerOptions);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(sp =>
{
    var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"))
    .DefaultIndex(SearchIndices.Episode);
    return new ElasticsearchClient(settings);
});

var connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:Default") ?? "";
builder.Services.AddCap(x =>
{
    x.UseMySql(connectionString);

    x.UseRabbitMQ(opt =>
    {
        opt.HostName = "localhost";
        opt.UserName = "rmquser";
        opt.Password = "rmqpassword";
        opt.Port = 5672;
        opt.ExchangeName = "SearchService";
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
