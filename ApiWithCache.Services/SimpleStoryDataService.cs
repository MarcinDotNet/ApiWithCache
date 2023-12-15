// Ignore Spelling: Api

using AspWithCache.Model.Interfaces;

namespace ApiWithCache.Services
{
    public class SimpleStoryDataService : IStoryDataService
    {
        private readonly IAspWithCacheLogger _logger;

        public SimpleStoryDataService(IAspWithCacheLogger logger)
        {
            _logger = logger;
        }

        public IStoryInformation[] GetStoryInformations(int limit)
        {
            return null;
        }
    }
}