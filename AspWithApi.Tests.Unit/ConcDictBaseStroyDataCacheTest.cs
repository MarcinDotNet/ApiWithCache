using ApiWithCache.Services.Caches;
using ApiWithCache.Services.Services;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AutoFixture;
using Moq;

namespace AspWithApi.Tests.Unit
{
    [TestClass]
    public class ConcDictBaseStroyDataCacheTest
    {
        [DataTestMethod]
        [DataRow(1, 2, 100)]
        [DataRow(5, 20, 100)]
        [DataRow(80, 200, 100)]
        public void Cache_ShouldNotFail_AfterProcessing(int workers, int callsPerWorker, int delayMultiplier)
        {
            string providerName = "test";
            Fixture fixture = new Fixture();
    
            Mock<IAspWithCacheLogger> logger = new Mock<IAspWithCacheLogger>();
            ConcDictBaseStroyDataCache cache = new ConcDictBaseStroyDataCache(logger.Object);          
            Task[] taskArray = new Task[workers+1];
            taskArray[workers] = Task.Factory.StartNew(() =>
            {
                for (int j = 0; j < callsPerWorker; j++)
                {
                    cache.SetStoriesForProvider(providerName, fixture.CreateMany<HackerDataStoryInformation>(30).Select(x => (IStoryInformation)x).ToList());
                    Task.Delay(delayMultiplier).Wait();
                }
            });
            Task.Delay(delayMultiplier).Wait();
            for (int i = 0; i < workers; i++)
            {
                taskArray[i] = Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < callsPerWorker; j++)
                    {
                        cache.GetStoriesForProvider(providerName);
                        Task.Delay(delayMultiplier).Wait();
                    }
                });
            }
          
            Task.WaitAll(taskArray);
        }
    }
}