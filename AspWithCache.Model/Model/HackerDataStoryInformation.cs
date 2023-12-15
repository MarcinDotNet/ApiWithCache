using AspWithCache.Model.Interfaces;
using System.Text.Json.Serialization;

namespace AspWithCache.Model.Model
{
    public class HackerDataStoryInformation : IStoryInformation
    {
        public HackerDataStoryInformation(string id) => Id = id ?? throw new ArgumentNullException(nameof(id));
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public DateTime? LastLoadTime { get; set; }
        public string? Title { get; set; }
        public string? Uri { get; set; }
        public string? PostedBy { get; set; }
        public DateTime? Time { get; set; }
        public int? CommentCount { get; set; }
        public int? Score { get; set; }
    }
}