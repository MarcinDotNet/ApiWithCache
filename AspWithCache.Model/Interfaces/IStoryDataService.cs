namespace AspWithCache.Model.Interfaces
{
    public interface IStoryDataService : IDisposable
    {
        IStoryInformation[] GetStoryInformations(int limit, string providerId);
    }
}