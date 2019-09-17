using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;
using BrowserHistoryGatherer.Utils;

namespace BrowserHistoryGatherer.Gathering {
    public class FirefoxFavoriteGatherer : IFavoriteGatherer {
        public string BrowserName => Constants.BrowserName_Firefox;
        private const string FIREFOX_DATA_PATH = @"Mozilla\Firefox\Profiles\";
        private const string DATABASE_NAME = "places.sqlite";
        private const long TimeToSecondDevideCount = 1000000;

        private static IEnumerable<string> GetFirefoxDatabasePaths() {
            string dataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                FIREFOX_DATA_PATH
            );

            if (!Directory.Exists(dataFolder)) {
                return Enumerable.Empty<string>();
            }

            return Directory.EnumerateFiles(dataFolder, DATABASE_NAME, SearchOption.AllDirectories);
        }

        private static List<FavoriteEntry> GetBookMarksFromDBPath(string dbPath) {
            var favorites = new List<FavoriteEntry>();
            DataTable favoriteDt = null;
            try {
                var getBookmarksCommand = new SQLiteCommand(
                    @"select bookmarks.title,places.url,bookmarks.dateAdded from 
                        moz_bookmarks as bookmarks, 
                        moz_places as places on bookmarks.fk = places.id;");

                favoriteDt = SqliteUtils.QueryDataTable(dbPath,getBookmarksCommand);

                for (int i = 0; i < favoriteDt.Rows.Count; i++) {
                    var row = favoriteDt.Rows[i];

                    var title = row["title"].ToString();
                    var time = DateUtils.GetTimeFromNewCentery( (long) row["dateAdded"] / TimeToSecondDevideCount);
                    var uri = new Uri(row["url"].ToString());

                    var favorite = new FavoriteEntry(uri, title, time);
                    favorites.Add(favorite);
                }
            }
            catch(Exception ex) {
                Logger.WriteLine(ex.Message);
                return favorites;
            }
            finally {
                favoriteDt?.Dispose();
            }

            return favorites;
        }

        public ICollection<FavoriteEntry> GetBrowserFavorites() {
            var favorites = new List<FavoriteEntry>();

            foreach (var dbPath in GetFirefoxDatabasePaths()) {
                favorites.AddRange(GetBookMarksFromDBPath(dbPath));
            }

            return favorites;
        }

        FirefoxFavoriteGatherer() { }

        public static FirefoxFavoriteGatherer Instance { get; } = new FirefoxFavoriteGatherer();
    }
}
