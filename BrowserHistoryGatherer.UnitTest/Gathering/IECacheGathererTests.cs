using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer.Gathering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering.Tests {
    [TestClass()]
    public class IECacheGathererTests {
        [TestMethod()]
        public void GetBrowserCachesTest() {
            var caches = IECacheGatherer.Instance.GetBrowserCaches();
            
            var cacheGroups = caches.GroupBy(p => p.Uri.ToString().Substring(0, 5));
        }
    }
}