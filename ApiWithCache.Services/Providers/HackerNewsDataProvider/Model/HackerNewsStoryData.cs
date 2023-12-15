// Ignore Spelling: Api

using System.Text.Json.Serialization;

namespace ApiWithCache.Services.Providers.HackerNewsDataProvider.Model
{
    public class HackerNewsStoryData
    {
        [JsonPropertyName("by")]
        public string? By { get; set; }
        [JsonPropertyName("id")]
        public long? Id { get; set; }
        [JsonPropertyName("descendants")]
        public int? Descendants { get; set; }
        [JsonPropertyName("score")]
        public int? Score { get; set; }
        [JsonPropertyName("title")]
        public string? Title { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
        [JsonPropertyName("time")]
        public long Time { get; set; }
    }
}