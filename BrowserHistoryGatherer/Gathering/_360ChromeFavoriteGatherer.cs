using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    public class _360ChromeFavoriteGatherer:ChromeFavoriteGatherer {
        public override string BrowserName => Constants.BrowserName_360Chrome;
        protected override string FullDataPath => Path.Combine(
#if DEBUG
           "D:\\AppData",
#else
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
#endif
           "360Chrome\\Chrome\\User Data");

        protected _360ChromeFavoriteGatherer() { }

        public static _360ChromeFavoriteGatherer Instance { get; } = new _360ChromeFavoriteGatherer();
    }
}
