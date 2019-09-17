using BrowserHistoryGatherer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    public interface IBrowserCacheGatherer {
        ICollection<CacheEntry> GetBrowserCaches();

        string BrowserName { get; }
    }
}
