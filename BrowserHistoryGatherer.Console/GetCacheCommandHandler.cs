using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;
using BrowserHistoryGatherer.Gathering;
using BrowserHistoryGatherer.Utils;
using Newtonsoft.Json.Linq;
using static BrowserHistoryGatherer.Constants;

namespace BrowserHistoryGatherer {
    class GetCacheCommandHandler : ICommandHandler {
        public const string JsonArrayName_Caches = "Caches";
        public void Handle(JObject browserObject, CommandHandlerArgs commandHandlerArgs) {
            if (!commandHandlerArgs.Commands.Contains(Constants.Command_GetCache)) {
                return;
            }

            var browserGatherer = Gatherers.AllCachesGatherers.FirstOrDefault(p => p.BrowserName == commandHandlerArgs.BrowserType);

            if(browserGatherer == null) {
                Console.WriteLine($"No gatherer matched the specifed name {commandHandlerArgs.BrowserType}");
                return;
            }

            ICollection<CacheEntry> caches = null;
            try {
                Console.WriteLine($"Gathering cache-{browserGatherer.BrowserName}");
                caches = browserGatherer.GetBrowserCaches();
                Console.WriteLine($"Gathering cache-{browserGatherer.BrowserName} done");
                if(caches == null || caches.Count == 0) {
                    return;
                }

            }
            catch(Exception ex) {
                Console.WriteLine($"Error occured Gathering cache-{browserGatherer.BrowserName}");
                return;
            }
            

            var cacheJArray = new JArray();
            foreach (var cache in caches) {
                var cacheObject = new JObject();
                cacheObject[JsonElemName_Uri] = cache.Uri;
                cacheObject[JsonElemName_Size] = cache.Size;
                cacheObject[JsonElemName_VisitCount] = cache.VisitCount;
                cacheObject[JsonElemName_LastVisitTime] = cache.LastVisitTime.GetTimeStampFromNewCentery();

                cacheJArray.Add(cacheObject);
            }

            browserObject[JsonArrayName_Caches] = cacheJArray;
        }
    }
}
