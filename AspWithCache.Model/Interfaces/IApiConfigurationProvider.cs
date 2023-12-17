// Ignore Spelling: Api

using AspWithCache.Model.Model.Configuration;

namespace AspWithCache.Model.Interfaces
{
    public interface IApiConfigurationProvider
    {
        ApiWithCacheConfiguration GetConfiguration();
    }
}