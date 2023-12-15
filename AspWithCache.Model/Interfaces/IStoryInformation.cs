namespace AspWithCache.Model.Interfaces
{
    public interface IStoryInformation
    {
        public string Id { get; set; }
        public DateTime? LastLoadTime { get; set; }
        public int? Score { get; set; }
    }
}