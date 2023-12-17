using ApiWithCache.Services;
using ApiWithCache.Services.Caches;
using ApiWithCache.Services.Services;
using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AspWithCache.Model.Model.Configuration;
using AutoFixture;
using Moq;
using System.Configuration.Provider;

namespace AspWithApi.Tests.Unit
{
    [TestClass]
    public class StoryDataServiceWithCacheTest
    {
        private static string _providerName = "test";
        private readonly Fixture fixture = new Fixture();
        private static readonly Mock<IAspWithCacheLogger> _logger = new Mock<IAspWithCacheLogger>();
        private readonly Mock<IApiConfigurationProvider> _configurationProvider = new Mock<IApiConfigurationProvider>();
        private readonly ConcDictBaseStroyDataCache cache = new ConcDictBaseStroyDataCache(_logger.Object);

        [TestMethod]
        public void CreateService_ShouldReturn_Items()
        {
            _configurationProvider.Setup(x => x.GetConfiguration()).Returns(new ApiWithCacheConfiguration
            {
                HackerRankApiProviderId = _providerName,
                DataProviderRefreshTimeInMilliseconds = 2000,
                DataProviderConfigurations = new DataProviderConfiguration[] { new DataProviderConfiguration() { ApiUrl = "url", NewsLimit = 30, ProviderId = _providerName, Type = ProviderType.HackerNews } }
            });
            cache.SetStoriesForProvider(_providerName, fixture.CreateMany<HackerDataStoryInformation>(30).Select(x => (IStoryInformation)x).ToList());
            StoryDataServiceWithCache service = new StoryDataServiceWithCache(_logger.Object, _configurationProvider.Object, cache);
            var returnValue = service.GetStoryInformations(10, _providerName);
            Assert.IsNotNull(returnValue);
            Task.Delay(2000).Wait();
        }

        [TestMethod]
        [ExpectedException(typeof(NoDataFromProviderException))]
        public void CreateService_Should_Throw_NoDataFromProviderException()
        {
            _configurationProvider.Setup(x => x.GetConfiguration()).Returns(new ApiWithCacheConfiguration
            {
                HackerRankApiProviderId = _providerName,
                DataProviderRefreshTimeInMilliseconds = 2000,
                DataProviderConfigurations = new DataProviderConfiguration[] { new DataProviderConfiguration() { ApiUrl = "url", NewsLimit = 30, ProviderId = _providerName, Type = ProviderType.HackerNews } }
            });

            StoryDataServiceWithCache service = new StoryDataServiceWithCache(_logger.Object, _configurationProvider.Object, cache);
            service.GetStoryInformations(100, "HackerNewsProvider");
            service.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateService_Should_Throw_ArgumentOutOfRangeException()
        {
            _configurationProvider.Setup(x => x.GetConfiguration()).Returns(new ApiWithCacheConfiguration
            {
                HackerRankApiProviderId = _providerName,
                DataProviderRefreshTimeInMilliseconds = 2000,
                DataProviderConfigurations = new DataProviderConfiguration[] { new DataProviderConfiguration() { ApiUrl = "url", NewsLimit = 30, ProviderId = _providerName, Type = ProviderType.HackerNews } }
            });

            StoryDataServiceWithCache service = new StoryDataServiceWithCache(_logger.Object, _configurationProvider.Object, cache);
            service.GetStoryInformations(500, "HackerNewsProvider");
            service.Dispose();
        }
    }
}