using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Data {
    public class CacheEntry {
        public Uri Uri { get; }
        public DateTime LastVisitTime { get; }
        public uint? VisitCount { get; }
        public long Size { get; }

        public CacheEntry(Uri uri,DateTime lastVisitTime,uint? visitCount,long size) {
            Uri = uri;
            LastVisitTime = lastVisitTime;
            VisitCount = visitCount;
            Size = size;
        }
    }
}
