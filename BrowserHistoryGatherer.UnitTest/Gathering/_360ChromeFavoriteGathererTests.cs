using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer.Gathering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering.Tests {
    [TestClass()]
    public class _360ChromeFavoriteGathererTests {
        [TestMethod()]
        public void GetFavoriteTest() {
            var favorites = _360ChromeFavoriteGatherer.Instance.GetBrowserFavorites();
        }
    }
}