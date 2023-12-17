using AspWithCache.Model.Model.Configuration;

namespace AspWithCache.Model.Interfaces
{
    public interface IStoriesProviderFactory
    {
        IStoriesProvider GetStoriesProvider(DataProviderConfiguration storiesProviderConfig);
    }
}