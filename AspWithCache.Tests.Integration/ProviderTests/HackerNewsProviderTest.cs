using ApiWithCache.Services.Providers.HackerNewsDataProvider;
using AspWithCache.Model.Model;
using AspWithCache.Tests.Integration.MockImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspWithCache.Tests.Integration
{
    [TestClass]
    public class HackerNewsProviderTest
    {
        private readonly string _apiUrl = @"https://hacker-news.firebaseio.com/v0/";

        [TestMethod]
        public async Task GetContractList_ShouldReturn_Items()
        {
            HackerNewsDataProvider provider = new HackerNewsDataProvider(new MockLogger(), "1", _apiUrl);
            var result = await provider.GetStoriesListAsync(600);
            Assert.IsTrue(result.Any(), "No stories returned");
        }

        [TestMethod]
        public async Task GetSingleContract_ShouldReturn_AllData()
        {
            HackerNewsDataProvider provider = new HackerNewsDataProvider(new MockLogger(), "1", _apiUrl);
            var result = await provider.GetStoryDataAsync(new HackerDataStoryInformation("21233041"));
            Assert.IsTrue(((HackerDataStoryInformation)result).PostedBy == "ismaildonmez", "Wrong author read.");
            Assert.IsTrue(((HackerDataStoryInformation)result).Time?.Hour == 13, "Wrong hour set.");
        }
    }
}