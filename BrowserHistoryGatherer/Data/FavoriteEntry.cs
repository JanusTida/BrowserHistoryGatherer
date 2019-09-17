using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Data {
    public class FavoriteEntry {
        public Uri Uri { get; }

        public string Title { get; }

        public DateTime? CreationTime { get; }

        public FavoriteEntry(Uri uri,string title,DateTime? creationTime) {
            Uri = uri;
            Title = title;
            CreationTime = creationTime;
        }
    }
}
