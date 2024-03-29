﻿// Ignore Spelling: api

using ApiWithCache.Services.Providers.HackerNewsDataProvider.Model;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model;
using System.Text.Json;

namespace ApiWithCache.Services.Providers.HackerNewsDataProvider
{
    public class HackerNewsDataProvider : IStoriesProvider
    {
        private const string storiesListUrl = "beststories.json";
        private const string storyDetailsUrlPrefix = @"item/";
        private const string storyDetailsUrlSuffix = ".json";
        private readonly string _apiUrl = "";
        private readonly IAspWithCacheLogger _logger;
        private readonly string _providerId;
        private readonly string _providerClassName;
        private readonly int _newsLimit;

        public HackerNewsDataProvider(IAspWithCacheLogger logger, string providerId, string apiUrl, int newsLimit)
        {
            if (string.IsNullOrEmpty(apiUrl)) throw new ArgumentNullException(nameof(apiUrl));
            _apiUrl = apiUrl;
            _logger = logger;
            _providerId = providerId;
            _providerClassName = this.GetType().Name;
            _logger.Info(providerId, _providerClassName, "Initialized");
            _newsLimit = newsLimit;
        }

        public int GetLimit()
        {
            return _newsLimit;
        }

        public string GetId()
        {
            return _providerId;
        }

        public async Task<IStoryInformation[]> GetStoriesListAsync(int limit)
        {
            var urlToQuery = _apiUrl + storiesListUrl;
            _logger.Info(_providerId, _providerClassName, "Querying for stories list with api " + urlToQuery);
            using (HttpClient client = new HttpClient())
            {
                using HttpResponseMessage response = await client.GetAsync(urlToQuery);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.Trace(_providerId, _providerClassName, "JsonResponse " + jsonResponse);
                var storiesList = JsonSerializer.Deserialize<int[]>(jsonResponse);
                if (storiesList == null) { return new HackerDataStoryInformation[] { }; }
                int limitNumber = _newsLimit > limit ? limit : _newsLimit;
                _logger.Warn(_providerId, _providerClassName, $"Returned rows will be limited to configuration limit {_newsLimit}");
                return storiesList.Select(x => new HackerDataStoryInformation(x.ToString())).Take(limitNumber).ToArray();
            }
        }

        public async Task<IStoryInformation> GetStoryDataAsync(IStoryInformation storyInformation)
        {
            var urlToQuery = _apiUrl + storyDetailsUrlPrefix + storyInformation.Id + storyDetailsUrlSuffix;
            _logger.Info(_providerId, _providerClassName, $"Querying for single story {storyInformation} :url {urlToQuery}");
            using (HttpClient client = new HttpClient())
            {
                using HttpResponseMessage response = await client.GetAsync(urlToQuery);
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.Trace(_providerId, _providerClassName, "JsonResponse " + jsonResponse);
                var storyDataFromApi = JsonSerializer.Deserialize<HackerNewsStoryData>(jsonResponse);
                if (storyDataFromApi == null) { throw new ArgumentOutOfRangeException("storyInformation", "Empty data loaded."); }
                var hackerNewstoryData = (HackerDataStoryInformation)storyInformation;
                hackerNewstoryData.Uri = storyDataFromApi.Url;
                hackerNewstoryData.LastLoadTime = DateTime.Now;
                hackerNewstoryData.Time = DateTimeOffset.FromUnixTimeSeconds(storyDataFromApi.Time).UtcDateTime;
                hackerNewstoryData.Score = storyDataFromApi.Score;
                hackerNewstoryData.CommentCount = storyDataFromApi.Descendants;
                hackerNewstoryData.PostedBy = storyDataFromApi.By;
                hackerNewstoryData.Title = storyDataFromApi.Title;
                return storyInformation;
            }
        }
    }
}