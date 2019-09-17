using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    /// <summary>
    /// 360安全浏览器;
    /// </summary>
    class _360SEGatherer:ChromeGatherer {
        public override string Name => Constants.BrowserName_360SE;
        protected override string FullDataPath => Path.Combine(
#if DEBUG
           "D:\\AppData",
#else
           Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
#endif
           "360se6\\User Data"
        );

        _360SEGatherer() { }

        public static readonly new _360SEGatherer Instance = new _360SEGatherer();
    }
}
