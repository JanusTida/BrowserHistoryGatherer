using BrowserHistoryGatherer.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrowserHistoryGatherer.Gathering {
    public class IEFavoriteGatherer : IFavoriteGatherer {
        IEFavoriteGatherer() {

        }

        public static IEFavoriteGatherer Instance { get; } = new IEFavoriteGatherer();
        public string BrowserName => Constants.BrowserName_IE;
        private const string RegistryKeyPath_Favorite = "Software\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Shell Folders";
        private const string RegistryKeyName_Favorites = "Favorites";

        private readonly string UriPrefix = "URL=";
        private const int MaxUrlFileSize = 1048576;

        /// <summary>
        /// 需要管理员权限;
        /// </summary>
        /// <returns></returns>
        public ICollection<FavoriteEntry> GetBrowserFavorites() {
            var favorites = new List<FavoriteEntry>();
            try {
                var key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath_Favorite);
                var favoritePath = key.GetValue(RegistryKeyName_Favorites) as string;
                if (string.IsNullOrEmpty(favoritePath)) {
                    return favorites;
                }

                var allUrlFiles = Directory.EnumerateFiles(favoritePath,"*.url");
                foreach (var urlFile in allUrlFiles) {
                    var fileInfo = new FileInfo(urlFile);
                    if(fileInfo.Length > MaxUrlFileSize) {
                        continue;
                    }

                    var title = Path.GetFileNameWithoutExtension(urlFile);
                    var allLines = File.ReadAllLines(fileInfo.FullName);

                    var uriLine = allLines.FirstOrDefault(p => p.StartsWith(UriPrefix));
                    if(uriLine == null) {
                        continue;
                    }
                    var uri = uriLine.Substring(UriPrefix.Length);

                    var entry = new FavoriteEntry(new Uri(uri), title, fileInfo.LastWriteTime);

                    favorites.Add(entry);
                }

                return favorites;
            }
            catch(Exception ex) {
                Logger.WriteLine(ex.Message);
                return favorites;
            }
        }
    }
}
