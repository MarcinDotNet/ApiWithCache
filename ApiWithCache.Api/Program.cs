using ApiWithCache.Services;
using ApiWithCache.Services.Caches;
using ApiWithCache.Services.Services;
using AspWithCache.Model.Interfaces;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
var builder = WebApplication.CreateBuilder(args);
IApiConfigurationProvider provider = new ApiConfigurationProvider();
IAspWithCacheLogger nlogLogger = new NlogLogger();
IStoriesProviderFactory factory = new StoriesProviderFactory(nlogLogger);
IStoryDataCache cache = new ConcDictBaseStroyDataCache(nlogLogger);
IStoryDataService service = new StoryDataServiceWithCache(nlogLogger, factory, provider, cache);
// Add services to the container.
builder.Services.AddSingleton(nlogLogger);
builder.Services.AddSingleton(provider);
builder.Services.AddSingleton(service);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.EnableAnnotations()
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();