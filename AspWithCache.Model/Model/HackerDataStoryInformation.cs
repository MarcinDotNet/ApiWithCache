using AspWithCache.Model.Interfaces;
using System.Xml.Linq;

namespace AspWithCache.Model.Model
{

    public class HackerDataStoryInformation : IStoryInformation
    {
        public HackerDataStoryInformation(string id) => Id = id ?? throw new ArgumentNullException(nameof(id));
        public string Id { get; set; } 
        public DateTime? LastLoadTime { get; set; }
        public string? Title { get; set; }
        public string? Uri { get; set; }
        public string? PostedBy { get; set; }
        public DateTime? Time { get; set; }
        public int? CommentCount { get; set; }
        public int? Score { get; set; }
    }

}