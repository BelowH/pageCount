using System.Reflection;
using pageCount;
using pageCount.auth;

string workingDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(workingDirectory, "appsettings.json"));
#if DEBUG
builder.Configuration.AddJsonFile(Path.Combine(workingDirectory, "appsettings.Development.json"));
#endif


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddTransient<IDatabaseRepository, DatabaseRepository>();
builder.Services.AddTransient<ICountService, CountService>();


WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<BasicAuthMiddleware>(app.Configuration);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();