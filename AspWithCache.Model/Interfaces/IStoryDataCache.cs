using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspWithCache.Model.Interfaces
{
    public interface IStoryDataCache : IDisposable
    {
        List<IStoryInformation> GetStoriesForProvider(string providerId);

        void SetStoriesForProvider(string providerId, List<IStoryInformation> newData);
    }
}