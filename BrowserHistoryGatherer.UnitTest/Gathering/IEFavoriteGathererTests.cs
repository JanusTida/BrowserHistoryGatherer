using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer.Gathering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering.Tests {
    [TestClass()]
    public class IEFavoriteGathererTests {
        [TestMethod()]
        public void GetBrowserFavoritesTest() {
            var gatherer = IEFavoriteGatherer.Instance;
            var favs = gatherer.GetBrowserFavorites();
        }
    }
}