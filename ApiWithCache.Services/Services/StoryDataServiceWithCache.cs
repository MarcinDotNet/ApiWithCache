// Ignore Spelling: Api

using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;

namespace ApiWithCache.Services.Services
{
    public class StoryDataServiceWithCache : IStoryDataService
    {
        private readonly IAspWithCacheLogger _logger;
        private readonly ApiWithCacheConfiguration _configuration;
        private readonly IStoryDataCache _cache;
        private readonly string _className;

        // To detect redundant calls
        private bool _disposedValue;

        public StoryDataServiceWithCache(IAspWithCacheLogger logger, IApiConfigurationProvider configurationProvider, IStoryDataCache cache)
        {
            _logger = logger;
            _configuration = configurationProvider.GetConfiguration();
            _className = nameof(SimpleStoryDataService);
            _cache = cache;
        }

        public IStoryInformation[] GetStoryInformations(int limit, string providerId)
        {
            if (!_configuration.DataProviderConfigurations.ToList().Exists(x => x.ProviderId == providerId)) throw new NotKnowProviderException($"Provider not known {providerId}");
            if (_configuration.DataProviderConfigurations.First(x => x.ProviderId == providerId).NewsLimit < limit) throw new ArgumentOutOfRangeException("limit");

            return _cache.GetStoriesForProvider(providerId).Take(limit).ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _logger.Trace(_className, $"Disposing finished.");
                }

                _disposedValue = true;
            }
        }
    }
}