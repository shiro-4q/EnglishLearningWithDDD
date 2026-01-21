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

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// 使用额外中间件
app.UseExtraMiddleware(initializerOptions);

app.MapControllers();

app.Run();
