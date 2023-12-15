// Ignore Spelling: Api

using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ApiWithCache.Services
{
    public class SimpleStoryDataService : IStoryDataService
    {
        private readonly IAspWithCacheLogger _logger;
        private IStoriesProviderFactory _providerFactory;
        private readonly ApiWithCacheConfiguration _configuration;
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _listenTask;
        private ConcurrentDictionary<string, List<IStoryInformation>> _dictionaryOfProviderResults = new System.Collections.Concurrent.ConcurrentDictionary<string, List<IStoryInformation>>();
        private readonly List<IStoriesProvider> storiesProviders = new List<IStoriesProvider>();

        public SimpleStoryDataService(IAspWithCacheLogger logger, IStoriesProviderFactory providerFactory, IApiConfigurationProvider configurationProvider)
        {

            _logger = logger;
            _providerFactory = providerFactory;
            _configuration = configurationProvider.GetConfiguration();
            _logger.Info($"SimpleStoryDataService || initialize providers");
            InitializeProviders();
        }

        private void InitializeProviders()
        {
            foreach (var providerConfiguration in _configuration.DataProviderConfigurations)
            {
                storiesProviders.Add(_providerFactory.GetStoriesProvider(providerConfiguration));
            }
        }

        public IStoryInformation[] GetStoryInformations(int limit, string providerId)
        {
            List<IStoryInformation> stories = new List<IStoryInformation>();
            if (_dictionaryOfProviderResults.TryGetValue(providerId, out stories))
            {
                return stories.OrderByDescending(x => x.Score).Take(limit).ToArray();
            }
            else
            {
                _logger.Warn(providerId, this.GetType().Name, "No data from provider");
                throw new NoDataFromProviderException($"No data from provider {providerId}");
            }
        }
    }
}