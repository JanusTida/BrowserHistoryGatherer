using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    public static class Gatherers {
        public static readonly IReadOnlyCollection<IBrowserHistoryGatherer> AllHistoryGatherers =
            new ReadOnlyCollection<IBrowserHistoryGatherer>(
                new IBrowserHistoryGatherer[] {
                    ChromeGatherer.Instance,
                    IEGatherer.Instance,
                    SafariGatherer.Instance,
                    FirefoxGatherer.Instance,
                    _360ChromeGatherer.Instance,
                    _360SEGatherer.Instance
                }
            );
            

        public static readonly IReadOnlyCollection<IBrowserCacheGatherer> AllCachesGatherers =
            new ReadOnlyCollection<IBrowserCacheGatherer>(
                new IBrowserCacheGatherer[]{
                    IECacheGatherer.Instance
                }
            );

        public static readonly IReadOnlyCollection<IFavoriteGatherer> AllFavoriteGatherers =
            new ReadOnlyCollection<IFavoriteGatherer>(
                new IFavoriteGatherer[] {
                    IEFavoriteGatherer.Instance,
                    ChromeFavoriteGatherer.Instance,
                    _360ChromeFavoriteGatherer.Instance,
                    _360SEFavoriteGatherer.Instance
                }
            );
    }
}
