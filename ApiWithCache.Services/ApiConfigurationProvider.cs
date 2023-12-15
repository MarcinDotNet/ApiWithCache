// Ignore Spelling: Api

using AspWithCache.Model.Exceptions;
using AspWithCache.Model.Interfaces;
using AspWithCache.Model.Model.Configuration;
using Microsoft.Extensions.Configuration;

namespace ApiWithCache.Services
{
    public class ApiConfigurationProvider : IApiConfigurationProvider
    {
        private readonly ApiWithCacheConfiguration _configuration;
        private static readonly ApiConfigurationProvider _configurationProviderInstance = new ApiConfigurationProvider();

        public ApiConfigurationProvider()
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json").Build();
            IConfigurationSection configurationSection = config.GetSection("ApiWithCacheConfiguration");
            var configuration = configurationSection.Get<ApiWithCacheConfiguration>();
            if (configuration != null) { _configuration = configuration; }
            else { throw new UnableToLoadConfigurationException("Configuration is null"); }
        }

        public ApiWithCacheConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public static ApiConfigurationProvider GetInstance() => _configurationProviderInstance;
    }
}