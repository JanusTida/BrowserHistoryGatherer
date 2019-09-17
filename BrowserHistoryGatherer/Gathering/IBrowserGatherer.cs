using BrowserHistoryGatherer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    public interface IBrowserHistoryGatherer {
        ICollection<HistoryEntry> GetBrowserHistories(DateTime? startTime, DateTime? endTime);

        string Name { get; }

        
    }
}
