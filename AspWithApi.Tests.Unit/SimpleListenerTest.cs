using ApiWithCache.Services.Caches;
using ApiWithCache.Services.Listeners;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AspWithCache.Model.Model.Configuration;
using AutoFixture;
using Moq;

namespace AspWithApi.Tests.Unit
{
    [TestClass]
    public class SimpleListenerTest
    {
        [DataTestMethod]
        [DataRow(200, 100)]
        [DataRow(500, 100)]
        [DataRow(800, 100)]
        public void StartListening_ShouldReturn_Items_AfterLimit(int delay, int refreshTime)
        {
            string providerName = "test";
            Fixture fixture = new Fixture();
            Mock<IAspWithCacheLogger> logger = new Mock<IAspWithCacheLogger>();
            Mock<IStoriesProvider> storiesProvider = new Mock<IStoriesProvider>();
            storiesProvider.Setup(x => x.GetStoriesListAsync(It.IsAny<int>()).Result).Returns(fixture.CreateMany<HackerDataStoryInformation>(30).ToArray());
            storiesProvider.Setup(x => x.GetStoryDataAsync(It.IsAny<IStoryInformation>()).Result).Returns(fixture.Create<HackerDataStoryInformation>());
            storiesProvider.Setup(x => x.GetId()).Returns(providerName);
            storiesProvider.Setup(x => x.GetLimit()).Returns(30);
            Mock<IStoriesProviderFactory> factory = new Mock<IStoriesProviderFactory>();
            factory.Setup(x => x.GetStoriesProvider(It.IsAny<DataProviderConfiguration>())).Returns(storiesProvider.Object);
            Mock<IApiConfigurationProvider> configurationProvider = new Mock<IApiConfigurationProvider>();

            configurationProvider.Setup(x => x.GetConfiguration()).Returns(new ApiWithCacheConfiguration
            {
                HackerRankApiProviderId = providerName,
                DataProviderRefreshTimeInMilliseconds = refreshTime,
                DataProviderConfigurations = new DataProviderConfiguration[] { new DataProviderConfiguration() { ApiUrl = "url", NewsLimit = 30, ProviderId = providerName, Type = ProviderType.HackerNews } }
            });
            ConcDictBaseStroyDataCache cache = new ConcDictBaseStroyDataCache(logger.Object);
            SimpleListener service = new SimpleListener(logger.Object, factory.Object, configurationProvider.Object, cache);
            service.Start();
            Task.Delay(delay).Wait();

            var returnValue = cache.GetStoriesForProvider(providerName);
            Assert.IsNotNull(returnValue);
            Task.Delay(2000).Wait();
        }
    }
}