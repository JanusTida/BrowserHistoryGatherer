using BrowserHistoryGatherer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    /// <summary>
    /// 收藏夹;
    /// </summary>
    public interface IFavoriteGatherer {
        string BrowserName { get; }

        ICollection<FavoriteEntry> GetBrowserFavorites();
    }
}
