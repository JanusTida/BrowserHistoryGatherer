using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Tests {
    [TestClass()]
    public class UrlHistoryDataProviderTests {
        [TestMethod()]
        public void LoadHistoryUrlsTest() {
            var urls = UrlHistoryDataProvider.LoadHistoryUrls();
        }
    }
}