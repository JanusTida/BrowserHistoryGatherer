using Microsoft.VisualStudio.TestTools.UnitTesting;
using BrowserHistoryGatherer.Gathering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering.Tests {
    [TestClass()]
    public class FirefoxFavoriteGathererTests {
        [TestMethod()]
        public void GetBrowserFavoritesTest() {
            var favorites = FirefoxFavoriteGatherer.Instance.GetBrowserFavorites();
            Assert.IsNotNull(favorites);
            Assert.AreNotEqual(favorites.Count, 0);
        }
    }
}