namespace AspWithCache.Model.Interfaces
{
    public interface IStoriesProvider
    {
        int GetLimit();

        string GetId();

        Task<IStoryInformation[]> GetStoriesListAsync(int limit);

        Task<IStoryInformation> GetStoryDataAsync(IStoryInformation storyInformation);
    }
}