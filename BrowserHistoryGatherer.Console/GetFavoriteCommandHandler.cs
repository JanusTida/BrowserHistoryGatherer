using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Gathering;
using Newtonsoft.Json.Linq;
using static BrowserHistoryGatherer.Constants;
using BrowserHistoryGatherer.Utils;
using BrowserHistoryGatherer.Data;

namespace BrowserHistoryGatherer {
    class GetFavoriteCommandHandler : ICommandHandler {
        public const string JsonArrayName_Favorites = "Favorites";
        public void Handle(JObject browserObject, CommandHandlerArgs commandHandlerArgs) {
            if (!commandHandlerArgs.Commands.Contains(Constants.Command_GetFavorite)) {
                return;
            }

            var browserGatherer = Gatherers.AllFavoriteGatherers.FirstOrDefault(p => commandHandlerArgs.BrowserType == p.BrowserName);

            if (browserGatherer == null) {
                Logger.WriteLine($"No gatherer matched the specifed name {commandHandlerArgs.BrowserType}");
                return;
            }

            ICollection<FavoriteEntry> favorites = null;
            try {
                Console.WriteLine($"Gathering favorite-{browserGatherer.BrowserName}");
                favorites = browserGatherer.GetBrowserFavorites();
                Console.WriteLine($"Gathering favorite-{browserGatherer.BrowserName} done");
                if(favorites == null || favorites.Count == 0) {
                    return;
                }
            }
            catch(Exception ex) {
                Console.WriteLine($"Error occured gathering history-{browserGatherer.BrowserName}:{ex.Message}");
                return;
            }
                
            if(favorites.Count == 0) {
                return;
            }

            var favoriteJArray = new JArray();
            foreach (var favorite in favorites) {
                var favoriteJObject = new JObject();
                favoriteJObject[JsonElemName_Uri] = favorite.Uri;
                favoriteJObject[JsonElemName_Title] = favorite.Title;
                favoriteJObject[JsonElemName_CreationTime] = favorite.CreationTime?.GetTimeStampFromNewCentery()??null;

                favoriteJArray.Add(favoriteJObject);
            }

            browserObject[JsonArrayName_Favorites] = favoriteJArray;
        }
    }
}
