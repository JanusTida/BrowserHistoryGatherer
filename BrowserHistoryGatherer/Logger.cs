using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer {
    public static class Logger {
        
        public static void WriteLine(string message) {
            var logFilePath =
                $"{Environment.CurrentDirectory}\\{DateTime.Now.ToLongDateString()}.log";

            using (var sw = new StreamWriter(logFilePath,true)) {
                sw.WriteLine(message);
            }
        }
    }
}
