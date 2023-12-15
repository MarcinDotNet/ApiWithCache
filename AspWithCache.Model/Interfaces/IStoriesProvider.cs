namespace AspWithCache.Model.Interfaces
{
    public interface IStoriesProvider
    {
        int GetLimit(int limit);
        
        Task<IStoryInformation[]> GetStoriesListAsync(int limit);

        Task<IStoryInformation> GetStoryDataAsync(IStoryInformation storyInformation);
    }
}