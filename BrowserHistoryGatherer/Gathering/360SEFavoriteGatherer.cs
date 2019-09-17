using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    class _360SEFavoriteGatherer:ChromeFavoriteGatherer {
        public override string BrowserName => Constants.BrowserName_360SE;
        protected override string FullDataPath => Path.Combine(
#if DEBUG
           "D:\\AppData",
#else
           Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
#endif
           "360se6\\User Data"
        );

        _360SEFavoriteGatherer() { }

        public static _360SEFavoriteGatherer Instance { get; } = new _360SEFavoriteGatherer();
    }
}
