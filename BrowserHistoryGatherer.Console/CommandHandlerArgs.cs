using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer {
    public struct CommandHandlerArgs {
        public string[] Commands { get; set; }
        public string BrowserType { get; set; }
    }
}
