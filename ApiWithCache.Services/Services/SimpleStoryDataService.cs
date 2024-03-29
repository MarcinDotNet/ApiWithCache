﻿// Ignore Spelling: Api

using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;
using System.Collections.Concurrent;

namespace ApiWithCache.Services.Services
{
    /// <summary>
    /// responsible for cache and listening - this should go to 2 different classes.
    /// </summary>
    public class SimpleStoryDataService : IStoryDataService
    {
        private readonly IAspWithCacheLogger _logger;
        private readonly IStoriesProviderFactory _providerFactory;
        private readonly ApiWithCacheConfiguration _configuration;
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private Task? _listenTask;
        private readonly ConcurrentDictionary<string, List<IStoryInformation>> _dictionaryOfProviderResults = new ConcurrentDictionary<string, List<IStoryInformation>>();
        private readonly List<IStoriesProvider> storiesProviders = new List<IStoriesProvider>();
        private readonly string _className;

        // To detect redundant calls
        private bool _disposedValue;

        public SimpleStoryDataService(IAspWithCacheLogger logger, IStoriesProviderFactory providerFactory, IApiConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _providerFactory = providerFactory;
            _configuration = configurationProvider.GetConfiguration();
            _className = nameof(SimpleStoryDataService);
            _logger.Info(_className, "Initialize providers");
            _cancellationTokenSource = new CancellationTokenSource();

            InitializeProviders();
            StartListening();
        }

        private void InitializeProviders()
        {
            storiesProviders.AddRange(from providerConfiguration in _configuration!.DataProviderConfigurations
                                      select _providerFactory.GetStoriesProvider(providerConfiguration));
        }

        public void StopListening()
        {
            _cancellationTokenSource.Cancel();
        }

        public void StartListening()
        {
            _logger.Info(_className, "Starting reading data");
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _listenTask = new Task(ReadDataFromProvider, _cancellationToken);
            _listenTask.Start();
            _logger.Info(_className, "Listening started");
        }

        public IStoryInformation[] GetStoryInformations(int limit, string providerId)
        {
            if (!_configuration.DataProviderConfigurations.ToList().Exists(x => x.ProviderId == providerId)) throw new NotKnowProviderException($"Provider not known {providerId}");
            if (_configuration.DataProviderConfigurations.First(x => x.ProviderId == providerId).NewsLimit < limit) throw new ArgumentOutOfRangeException("limit");

            List<IStoryInformation>? stories = null;
            if (_dictionaryOfProviderResults.TryGetValue(providerId, out stories))
            {
                return stories.OrderByDescending(x => x.Score).Take(limit).ToArray();
            }
            else
            {
                _logger.Warn(providerId, GetType().Name, "No data from provider");
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
                        try
                        {
                            if (_cancellationToken.IsCancellationRequested) continue;
                            _logger.Info(_className, $"Refreshing provider data {storyProvider.GetId()}");
                            List<IStoryInformation> storiesList = new List<IStoryInformation>();
                            var task = storyProvider.GetStoriesListAsync(storyProvider.GetLimit());
                            task.Wait();
                            storiesList.AddRange(task.Result);
                            List<IStoryInformation> storiesFilled = new List<IStoryInformation>();

                            _logger.Info(_className, $"Reading stories for   {storyProvider.GetId()}  number of stories {storiesFilled.Count}");
                            int count = 1;
                            int storiesToRead = storiesList.Count;
                            foreach (var item in storiesList)
                            {
                                if (_cancellationToken.IsCancellationRequested) break;
                                _logger.Info(_className, $"Reading story for   {storyProvider.GetId()}    {count}  of  {storiesToRead}");
                                var storyTask = storyProvider.GetStoryDataAsync(item);
                                storyTask.Wait();
                                storiesFilled.Add(storyTask.Result);
                                count++;
                            }
                            if (_cancellationToken.IsCancellationRequested) continue;
                            storiesFilled = storiesFilled.OrderByDescending(x => x.Score).ToList();
                            _logger.Info(_className, $"All stories loaded   {storyProvider.GetId} updating dictionary with new set");
                            _dictionaryOfProviderResults.AddOrUpdate(storyProvider.GetId(), storiesFilled, (_, _) => storiesFilled);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(storyProvider.GetId(), _className, "ReadDataFromProvider", $"Error on loading data, current data update  will be skipped for {storyProvider.GetId()} ",ex);
                        }
                    }

                    if (_cancellationToken.IsCancellationRequested)
                    {
                        shouldStillListen = false;
                    }
                    try
                    {
                        Task.Delay(_configuration.DataProviderRefreshTimeInMilliseconds, _cancellationToken).Wait();
                    }
                    catch (Exception)
                    {
                        shouldStillListen = false;
                    }
                }
            }

            _logger.Trace(_className, $"listener stopped.");
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
                    _cancellationTokenSource.Cancel();
                    _listenTask?.Wait();
                    _logger.Trace(_className, $"Disposing finished.");
                }

                _disposedValue = true;
            }
        }
    }
}