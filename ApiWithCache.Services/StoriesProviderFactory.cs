// Ignore Spelling: Api

using ApiWithCache.Services.Providers.HackerNewsDataProvider;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AspWithCache.Model.Model.Configuration;

namespace ApiWithCache.Services
{
    public class StoriesProviderFactory : IStoriesProviderFactory
    {
        private readonly IAspWithCacheLogger _logger;

        public StoriesProviderFactory(IAspWithCacheLogger logger) {
        _logger= logger;
        }
        public IStoriesProvider GetStoriesProvider(DataProviderConfiguration storiesProviderConfig)
        {
            switch (storiesProviderConfig.Type)
            {
                case ProviderType.HackerNews:
                    return new HackerNewsDataProvider(_logger, storiesProviderConfig.ProviderId,storiesProviderConfig.ApiUrl,storiesProviderConfig.NewsLimit);
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(storiesProviderConfig),"Not supported provider type.");
                  
            }
        }
    }
}