namespace AspWithCache.Model.Interfaces
{
    public interface IStoriesProvider
    {
        Task<IStoryInformation[]> GetStoriesListAsync(int limit);

        Task<IStoryInformation> GetStoryDataAsync(IStoryInformation storyInformation);
    }
}