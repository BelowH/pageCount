using System.Reflection;
using pageCount;
using pageCount.Middleware;

string workingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//builder.Configuration.AddJsonFile(Path.Combine(workingDirectory, "appsettings.json"));
#if DEBUG
builder.Configuration.AddJsonFile(Path.Combine(workingDirectory, "appsettings.Development.json"));
#endif
builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICountService, CountService>();
builder.Services.AddTransient<IResultService, ResultService>();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<BasicAuthMiddleware>(app.Configuration);
app.UseMiddleware<RateLimitingMiddleware>(app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();