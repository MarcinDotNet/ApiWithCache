namespace AspWithCache.Model.Interfaces
{
    public interface IStoryDataService
    {
        IStoryInformation[] GetStoryInformations(int limit);
    }
}