// Ignore Spelling: Api

using ApiWithCache.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspWithCache.Tests.Integration.Configuration
{
    [TestClass]
    public class ApiConfigurationProviderTest
    {
        [TestMethod]
        public void ReadConfigFromFilesTest_ShouldBeOk()
        {
            ApiConfigurationProvider providerInstance = ApiConfigurationProvider.GetInstance();
            Assert.IsNotNull(providerInstance);
            Assert.IsNotNull(providerInstance.GetConfiguration());
        }
    }
}