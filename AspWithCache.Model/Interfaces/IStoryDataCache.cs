namespace AspWithCache.Model.Interfaces
{
    public interface IStoryDataCache : IDisposable
    {
        List<IStoryInformation> GetStoriesForProvider(string providerId);

        void SetStoriesForProvider(string providerId, List<IStoryInformation> newData);
    }
}