using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer.Gathering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering.Tests {
    [TestClass()]
    public class ChromeFavoriteGathererTests {
        [TestMethod()]
        public void GetBrowserFavoritesTest() {
            var histories = ChromeFavoriteGatherer.Instance.GetBrowserFavorites();
            Assert.IsNotNull(histories);
        }
    }
}