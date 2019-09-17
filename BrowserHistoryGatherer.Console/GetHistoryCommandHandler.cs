using BrowserHistoryGatherer.Data;
using BrowserHistoryGatherer.Gathering;
using BrowserHistoryGatherer.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static BrowserHistoryGatherer.Constants;

namespace BrowserHistoryGatherer {
    class GetHistoryCommandHandler : ICommandHandler {

        public const string JsonArrayName_Histories = "Histories";


        public void Handle(JObject browserJObject, CommandHandlerArgs commandHandlerArgs) {
            if (!commandHandlerArgs.Commands.Contains(Command_GetHistory)) {
                return;
            }

            var browserGatherer = Gatherers.AllHistoryGatherers.FirstOrDefault(p => commandHandlerArgs.BrowserType == p.Name);

            if(browserGatherer == null) {
                Logger.WriteLine($"No gatherer matched the specifed name {commandHandlerArgs.BrowserType}");
                return;
            }

            ICollection<HistoryEntry> histories = null;

            
            try {
                Console.WriteLine($"Gathering history-{browserGatherer.Name}");
                histories = browserGatherer.GetBrowserHistories(null, null);
                Console.WriteLine($"Gathering history-{browserGatherer.Name} done");
                if (histories == null || histories.Count == 0) {
                    return;
                }
            }
            catch(Exception ex) {
                Console.WriteLine($"Error occured gathering history-{browserGatherer.Name}:{ex.Message}");
                Logger.WriteLine(ex.Message);
                return;
            }
            

            var historyJArray = new JArray();
            foreach (var history in histories) {
                var historyJObject = new JObject();
                historyJObject[JsonElemName_Uri] = history.Uri;
                historyJObject[JsonElemName_Title] = history.Title;
                historyJObject[JsonElemName_LastVisitTime] = history.LastVisitTime.GetTimeStampFromNewCentery();
                historyJObject[JsonElemName_VisitCount] = history.SafeVisitCount;

                historyJArray.Add(historyJObject);
            }
            browserJObject[JsonArrayName_Histories] = historyJArray;
        }
    }
}
