using FileService.Infrastructure.Adapters;
using FileService.Infrastructure.Persistence;
using Q.Initializer;

var builder = WebApplication.CreateBuilder(args);
// 注册额外服务
var initializerOptions = new InitializerOptions
{
    SwaggerTitle = "FileService.API V1"
};
builder.ConfigureExtraServices(initializerOptions);
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddCap(x =>
{
    // Outbox + 事务控制
    x.UseEntityFramework<FSDbContext>();

    // EventBus 使用 RabbitMQ
    x.UseRabbitMQ(opt =>
    {
        opt.HostName = "localhost";      // RabbitMQ 服务器地址
        opt.UserName = "rmquser";        // 登录用户名
        opt.Password = "rmqpassword";    // 登录密码
        opt.Port = 5672;                 // RabbitMQ 服务端口（5672 是默认 AMQP 端口）
    });

    // 重试配置
    x.FailedRetryCount = 5;
    x.FailedRetryInterval = 30;
});

// 配置本地存储选项
builder.Services.AddOptions<LocalStorageOptions>().Bind(builder.Configuration.GetSection("LocalStorageOptions"));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// 使用额外中间件
app.UseExtraMiddleware(initializerOptions);

app.MapControllers();

app.Run();
