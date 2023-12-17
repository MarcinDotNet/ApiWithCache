using ApiWithCache.Services.Services;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;

namespace ApiWithCache.Services.Listeners
{
    public class SimpleListener : IListenerStrategy
    {
        private readonly IAspWithCacheLogger _logger;
        private readonly IStoriesProviderFactory _providerFactory;
        private readonly ApiWithCacheConfiguration _configuration;
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IStoryDataCache _cache;
        private Task? _listenTask;
        private readonly List<IStoriesProvider> storiesProviders = new List<IStoriesProvider>();
        private readonly string _className;

        // To detect redundant calls
        private bool _disposedValue;

        public SimpleListener(IAspWithCacheLogger logger, IStoriesProviderFactory providerFactory, IApiConfigurationProvider configurationProvider, IStoryDataCache cache)
        {
            _logger = logger;
            _providerFactory = providerFactory;
            _configuration = configurationProvider.GetConfiguration();
            _className = nameof(SimpleStoryDataService);
            _logger.Info(_className, "Initialize providers");
            _cancellationTokenSource = new CancellationTokenSource();
            _cache = cache;
            InitializeProviders();
        }

        public void Start()
        {
            StartListening();
        }

        private void InitializeProviders()
        {
            storiesProviders.AddRange(from providerConfiguration in _configuration!.DataProviderConfigurations
                                      select _providerFactory.GetStoriesProvider(providerConfiguration));
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

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
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
                            _cache.SetStoriesForProvider(storyProvider.GetId(), storiesFilled);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(storyProvider.GetId(),_className, "ReadDataFromProvider", $"Error on loading data, current data update  will be skipped for {storyProvider.GetId()} ",ex);
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