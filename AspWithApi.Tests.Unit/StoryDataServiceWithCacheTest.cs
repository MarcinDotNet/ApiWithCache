using ApiWithCache.Services.Caches;
using ApiWithCache.Services.Services;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AspWithCache.Model.Model.Configuration;
using AutoFixture;
using Moq;

namespace AspWithApi.Tests.Unit
{
    [TestClass]
    public class StoryDataServiceWithCacheTest
    {
        [DataTestMethod]
        [DataRow(1, 2, 100)]
        [DataRow(5, 20, 100)]
        [DataRow(80, 200, 100)]
        public void CreateService_ShouldReturn_Items_AfterLimit(int workers, int callsPerWorker, int delayMultiplier)
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
                DataProviderRefreshTimeInMilliseconds = 2000,
                DataProviderConfigurations = new DataProviderConfiguration[] { new DataProviderConfiguration() { ApiUrl = "url", NewsLimit = 30, ProviderId = providerName, Type = ProviderType.HackerNews } }
            });
            ConcDictBaseStroyDataCache cache = new ConcDictBaseStroyDataCache(logger.Object);
            StoryDataServiceWithCache service = new StoryDataServiceWithCache(logger.Object, factory.Object, configurationProvider.Object, cache);

            Task.Delay(callsPerWorker).Wait();
            Task[] taskArray = new Task[workers];
            for (int i = 0; i < workers; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < callsPerWorker; j++)
                    {
                        service.GetStoryInformations(10, providerName);
                        Task.Delay(delayMultiplier).Wait();
                    }
                });
            }
            Task.WaitAll(taskArray);

            var returnValue = service.GetStoryInformations(10, providerName);
            Assert.IsNotNull(returnValue);
            Task.Delay(2000).Wait();
        }
    }
}