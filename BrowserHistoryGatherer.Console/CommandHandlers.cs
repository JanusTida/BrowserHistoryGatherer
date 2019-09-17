using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer {
    static class CommandHandlers {
        public static readonly ICommandHandler[] AllCommandHandlers =
            new ICommandHandler[] {
                new GetHistoryCommandHandler(),
                new GetCacheCommandHandler(),
                new GetFavoriteCommandHandler()
            };
    }
}
