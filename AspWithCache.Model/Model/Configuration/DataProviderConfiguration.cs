// Ignore Spelling: Api

namespace AspWithCache.Model.Model.Configuration
{
    public class DataProviderConfiguration
    {
        public string ProviderId { get; set; }
        public ProviderType Type { get; set; }
        public string ApiUrl { get; set; }
        
        public int NewsLimit { get; set; }
    }
}