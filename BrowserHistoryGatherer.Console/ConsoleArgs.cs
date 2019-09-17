using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer {
    struct ConsoleArgs {
        public string[] Commands { get; set; }

        public string[] BrowserTypes { get; set; }

        public string OutputFilePath { get; set; }
    }
}
