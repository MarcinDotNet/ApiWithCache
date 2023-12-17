using ApiWithCache.Services;
using ApiWithCache.Services.Providers.HackerNewsDataProvider;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using AspWithCache.Tests.Integration.MockImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspWithCache.Tests.Integration
{
    [TestClass]
    public class HackerNewsProviderTest
    {
        private readonly string _apiUrl = @"https://hacker-news.firebaseio.com/v0/";
        private readonly IAspWithCacheLogger _logger = new MockLogger();

        [TestMethod]
        public async Task GetContractList_ShouldReturn_Items()
        {
            HackerNewsDataProvider provider = new HackerNewsDataProvider(_logger, "1", _apiUrl, 500);
            var result = await provider.GetStoriesListAsync(600);
            Assert.IsTrue(result.Any(), "No stories returned");
        }

        [TestMethod]
        public async Task GetSingleContract_ShouldReturn_AllData()
        {
            HackerNewsDataProvider provider = new HackerNewsDataProvider(_logger, "1", _apiUrl, 500);
            var result = await provider.GetStoryDataAsync(new HackerDataStoryInformation("21233041"));
            Assert.IsTrue(((HackerDataStoryInformation)result).PostedBy == "ismaildonmez", "Wrong author read.");
            Assert.IsTrue(((HackerDataStoryInformation)result).Time?.Hour == 13, "Wrong hour set.");
        }

        [TestMethod]
        public async Task GetContractList_WithFactoryCreate_ShouldReturn_Items()
        {
            var providerConfiguration = ApiConfigurationProvider.GetInstance().GetConfiguration();
            StoriesProviderFactory storiesProviderFactory = new StoriesProviderFactory(_logger);
            IStoriesProvider provider = storiesProviderFactory.GetStoriesProvider(providerConfiguration.DataProviderConfigurations!.First(x => x.Type == ProviderType.HackerNews));
            var result = await provider.GetStoriesListAsync(600);
            Assert.IsTrue(result.Any(), "No stories returned");
        }
    }
}