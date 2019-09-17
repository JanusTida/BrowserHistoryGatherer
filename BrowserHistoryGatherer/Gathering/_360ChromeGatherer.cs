using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;

namespace BrowserHistoryGatherer.Gathering {
    /// <summary>
    /// 360极速浏览器;
    /// </summary>
    class _360ChromeGatherer : ChromeGatherer {
        public override string Name => Constants.BrowserName_360Chrome;

        protected override string FullDataPath => Path.Combine(
#if DEBUG
           "D:\\AppData",
#else
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
#endif
           "360Chrome\\Chrome\\User Data");

        _360ChromeGatherer() {

        }

        public new static _360ChromeGatherer Instance { get; } = new _360ChromeGatherer();
    }
}
