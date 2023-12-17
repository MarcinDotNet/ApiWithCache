using ApiWithCache.Services.Services;
using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiWithCache.Services.Caches
{
    public class ConcDictBaseStroyDataCache : IStoryDataCache
    {
        private readonly IAspWithCacheLogger _logger;
        private readonly ConcurrentDictionary<string, List<IStoryInformation>> _dictionaryOfProviderResults = new ConcurrentDictionary<string, List<IStoryInformation>>();
        private readonly string _className;
        private bool disposedValue;

        public ConcDictBaseStroyDataCache(IAspWithCacheLogger logger)
        {
            _logger = logger;
            _className = nameof(SimpleStoryDataService);
            _logger.Info(_className, "Initialize cache provider");
        }


        public List<IStoryInformation> GetStoriesForProvider(string providerId)
        {
            _logger.Trace(_className, "Reading data from provider");
            List<IStoryInformation>? stories = null;
            if (_dictionaryOfProviderResults.TryGetValue(providerId, out stories))
            {
                return stories.OrderByDescending(x => x.Score).ToList();
            }
            else
            {
                _logger.Warn(providerId, _className, "No data from provider");
                throw new NoDataFromProviderException($"No data from provider {providerId}");
            }
        }

        public void SetStoriesForProvider(string providerId, List<IStoryInformation> newData)
        {
            _logger.Trace(_className, "Setting data for provider");
            _dictionaryOfProviderResults.AddOrUpdate(providerId, newData, (_, _) => newData);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dictionaryOfProviderResults.Clear();
                }

                disposedValue = true;
            }
        }



        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
