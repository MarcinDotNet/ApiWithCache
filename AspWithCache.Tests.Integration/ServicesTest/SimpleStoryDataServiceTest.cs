// Ignore Spelling: Api

using ApiWithCache.Services;
using ApiWithCache.Services.Services;
using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Tests.Integration.MockImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspWithCache.Tests.Integration.ServicesTest
{
    [TestClass]
    public class SimpleStoryDataServiceTest
    {
        private readonly IAspWithCacheLogger _logger = new MockLogger();

        [TestMethod]
        [ExpectedException(typeof(NoDataFromProviderException))]
        public void CreateService_Should_Throw_NoDataFromProviderException()
        {
            StoriesProviderFactory storiesProviderFactory = new StoriesProviderFactory(_logger);
            SimpleStoryDataService service = new SimpleStoryDataService(_logger, storiesProviderFactory, ApiConfigurationProvider.GetInstance());
            service.GetStoryInformations(100, "HackerNewsProvider");
            service.Dispose();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void CreateService_Should_Throw_ArgumentOutOfRangeException()
        {
            StoriesProviderFactory storiesProviderFactory = new StoriesProviderFactory(_logger);
            SimpleStoryDataService service = new SimpleStoryDataService(_logger, storiesProviderFactory, ApiConfigurationProvider.GetInstance());
            service.GetStoryInformations(500, "HackerNewsProvider");
            service.Dispose();
        }

        [TestMethod]
        public void CreateService_ShouldReturn_Items_AfterTime()
        {
            StoriesProviderFactory storiesProviderFactory = new StoriesProviderFactory(_logger);
            SimpleStoryDataService service = new SimpleStoryDataService(_logger, storiesProviderFactory, ApiConfigurationProvider.GetInstance());
            Task.Delay(20000).Wait();
            var returnValue = service.GetStoryInformations(10, "HackerNewsProvider");
            Assert.IsNotNull(returnValue);
            service.Dispose();
            Task.Delay(2000).Wait();
        }

        [DataTestMethod]
        [DataRow(1, 10, 1000)]
        [DataRow(5, 20, 1000)]
        [DataRow(80, 20, 1000)]
        public void CreateService_ShouldReturn_Items_AfterLimit(int workers, int callsPerWorker, int delayMultiplier)
        {
            StoriesProviderFactory storiesProviderFactory = new StoriesProviderFactory(_logger);
            SimpleStoryDataService service = new SimpleStoryDataService(_logger, storiesProviderFactory, ApiConfigurationProvider.GetInstance());
            Task.Delay(20000).Wait();
            Task[] taskArray = new Task[workers];
            for (int i = 0; i < workers; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < callsPerWorker; j++)
                    {
                        service.GetStoryInformations(10, "HackerNewsProvider");
                        Task.Delay(delayMultiplier).Wait();
                    }
                });
            }
            Task.WaitAll(taskArray);

            var returnValue = service.GetStoryInformations(10, "HackerNewsProvider");
            Assert.IsNotNull(returnValue);
            Task.Delay(2000).Wait();
        }
    }
}