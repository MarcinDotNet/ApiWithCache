// Ignore Spelling: Api

using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ApiWithCache.Services
{
    public class SimpleStoryDataService : IStoryDataService, IDisposable
    {
        private readonly IAspWithCacheLogger _logger;
        private IStoriesProviderFactory _providerFactory;
        private readonly ApiWithCacheConfiguration _configuration;
        private readonly CancellationToken _cancellationToken;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _listenTask;
        private ConcurrentDictionary<string, List<IStoryInformation>> _dictionaryOfProviderResults = new System.Collections.Concurrent.ConcurrentDictionary<string, List<IStoryInformation>>();
        private readonly List<IStoriesProvider> storiesProviders = new List<IStoriesProvider>();
        private readonly string _className;

        public SimpleStoryDataService(IAspWithCacheLogger logger, IStoriesProviderFactory providerFactory, IApiConfigurationProvider configurationProvider)
        {

            _logger = logger;
            _providerFactory = providerFactory;
            _configuration = configurationProvider.GetConfiguration();
            _className = nameof(SimpleStoryDataService);
            _logger.Info(_className, "Initialize providers");
            _cancellationTokenSource = new CancellationTokenSource();
            
            InitializeProviders();

        }

        private void InitializeProviders()
        {
            storiesProviders.AddRange(from providerConfiguration in _configuration!.DataProviderConfigurations
                                      select _providerFactory.GetStoriesProvider(providerConfiguration));
        }

        private void StartListening()
        {
            _logger.Info("SimpleStoryDataService", "starting listening");

        }

        public IStoryInformation[] GetStoryInformations(int limit, string providerId)
        {
            List<IStoryInformation>? stories = null;
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

        private void ReadDataFromProvider()
        {
            bool shouldStillListen = !_cancellationTokenSource.IsCancellationRequested;

            if (shouldStillListen)
            {


                while (shouldStillListen)
                {
                    foreach (var storyProvider in storiesProviders)
                    {
                        //var task = storyProvider.GetStoriesListAsync(storyProvider.GetLimit()); ;
                        //task.Wait();
                        //var stories = await 


                    }
                }
            }

            _logger.Trace( _className, $"listener  stopped.");

        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}