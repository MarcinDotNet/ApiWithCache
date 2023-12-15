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
            ApiWithCacheConfiguration configuration = new ApiWithCacheConfiguration();
            configuration.DataProviderConfigurations = config.GetSection("ApiWithCacheConfiguration").GetSection("DataProviderConfigurations").Get<DataProviderConfiguration[]>();
            _configuration = configuration;
        }

        public ApiWithCacheConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public static ApiConfigurationProvider GetInstance() => _configurationProviderInstance;
    }
}