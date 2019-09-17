using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Tida.Util;

namespace BrowserHistoryGatherer.Gathering {
    /// <summary>
    /// Chrome收藏夹;
    /// </summary>
    public class ChromeFavoriteGatherer : IFavoriteGatherer {
        public virtual string BrowserName => Constants.BrowserName_Chrome;
        private const string DATA_PATH = @"Google\Chrome\User Data";
        private const string BookMarkFileName = "Bookmarks";
        private static readonly DateTime NTStartDateTime = new DateTime(1601, 1, 1, 0, 0, 0);
        private const long TimeToMillisecondDevideCount = 10000;

        private const string JsonElemName_Name = "name";
        private const string JsonElemName_DateAdded = "date_added";
        private const string JsonElemName_Url = "url";

        private string _fullDataPath;
        protected virtual string FullDataPath => _fullDataPath ?? (_fullDataPath = Path.Combine(
            //"D:\\AppData",
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            DATA_PATH));


        public ICollection<FavoriteEntry> GetBrowserFavorites() {
            var favoriteEntries = new List<FavoriteEntry>();
            if (!Directory.Exists(FullDataPath)) {
                return favoriteEntries;
            }

            var allBookMarks = Directory.EnumerateFiles(FullDataPath, BookMarkFileName, SearchOption.AllDirectories);
            foreach (var bookMarkFile in allBookMarks) {
                AddBookMarksToFavoriteList(favoriteEntries, bookMarkFile);
            }
            return favoriteEntries;
        }

        /// <summary>
        /// 添加指定收藏夹文件中的内容到列表中;
        /// </summary>
        /// <param name="favoriteEntries"></param>
        /// <param name="bookMarkFilePath"></param>
        private static void AddBookMarksToFavoriteList(List<FavoriteEntry> favoriteEntries,string bookMarkFilePath) {
            JsonReader jsonReader = null;
            TextReader textReader = null;
            try {
                textReader = new StreamReader(bookMarkFilePath);
                jsonReader = new JsonTextReader(textReader);
                var jObject = JObject.Load(jsonReader);
                var tokens = TreeUtils.BreadthFirstTraverse<JToken>(jObject, p => {
                    if (p is JContainer jContainer) {
                        return jContainer.Children();
                    }
                    return Enumerable.Empty<JToken>();
                });

                foreach (var token in tokens) {
                    if(!(token is JObject bookMarkObject)) {
                        continue;
                    }

                    if(bookMarkObject[JsonElemName_Name] == null) {
                        continue;
                    }

                    if(bookMarkObject[JsonElemName_DateAdded] == null) {
                        continue;
                    }

                    if(bookMarkObject[JsonElemName_Url] == null) {
                        continue;
                    }

                    var uri = new Uri(bookMarkObject[JsonElemName_Url].ToString());
                    var name = bookMarkObject[JsonElemName_Name].ToString();
                    DateTime? dateAdded = null;

                    if (long.TryParse(bookMarkObject[JsonElemName_DateAdded].ToString(), out var dateTimeStamp)) {
                        dateAdded = NTStartDateTime.AddMilliseconds(dateTimeStamp / 1000);
                    }

                    var favoriteEntry = new FavoriteEntry(uri, name, dateAdded);

                    favoriteEntries.Add(favoriteEntry);
                }
            }
            catch (Exception ex) {
                Logger.WriteLine(ex.Message);
            }
            finally {
                jsonReader?.Close();
                textReader?.Dispose();
            }


            
        }

        protected ChromeFavoriteGatherer() { }

        public static ChromeFavoriteGatherer Instance { get; } = new ChromeFavoriteGatherer();
    }
}
