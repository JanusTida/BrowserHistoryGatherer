using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;

namespace BrowserHistoryGatherer.Gathering {
    class FirefoxFavoriteGatherer : IFavoriteGatherer {
        public string BrowserName => Constants.BrowserName_Firefox;

        public ICollection<FavoriteEntry> GetBrowserFavorites() {
            return null;
        }
    }
}
