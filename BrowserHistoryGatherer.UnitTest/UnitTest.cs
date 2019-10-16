using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrowserHistoryGatherer.UnitTest {
    [TestClass]
    public class UnitTest {
        [TestMethod]
        public void TestDir() {
            var s = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            var s2 = Path.GetFileNameWithoutExtension("D://1.jpg");
            
        }
        [TestMethod]
        public void TestMethod1() {
            var firefoxHistories = BrowserHistory.GetHistory(Browser.Firefox);
            var chromeHistories = BrowserHistory.GetHistory(Browser.Chrome);
            var ieHistories = BrowserHistory.GetHistory(Browser.InternetExplorer);
            var safariHistories = BrowserHistory.GetHistory(Browser.Safari);
            var allHistories = BrowserHistory.GetHistory(Browser.All);
        }
    }
}
