using BrowserHistoryGatherer.Data;
using BrowserHistoryGatherer.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace BrowserHistoryGatherer.Gathering
{
    /// <summary>
    /// A gatherer to get chrome history entries
    /// </summary>
    internal class ChromeGatherer : IBrowserHistoryGatherer {
        #region Private Members
        protected virtual string FullDataPath => Path.Combine(
#if DEBUG
           "D:\\AppData",
#else
           Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
#endif
           CHROME_DATA_PATH);

        private const string CHROME_DATA_PATH = @"Google\Chrome\User Data";
        private const string DEFAULT_PROFILE_NAME = "Default";
        private const string CUSTOM_PROFILE_PATTERN = "Profile ?*";
        private const string DATABASE_NAME = "History";
        private const string TABLE_NAME = "urls";


        private IEnumerable<string> _chromeDatabasePaths;

        #endregion


        #region Public Properties
        public virtual string Name => Constants.BrowserName_Chrome;
        public static ChromeGatherer Instance { get; } = new ChromeGatherer();

        #endregion


        #region Events

        #endregion



        protected ChromeGatherer()
        {
            this._chromeDatabasePaths = GetChromeDatabasePaths();
        }



        public ICollection<HistoryEntry> GetBrowserHistories(DateTime? startTime, DateTime? endTime)
        {
            List<HistoryEntry> entryList = new List<HistoryEntry>();

            string query = string.Format("SELECT url, title, visit_count, datetime(last_visit_time/1000000-11644473600, 'unixepoch') AS last_visit " +
                                         "FROM {0}", TABLE_NAME);

            foreach (string dbPath in _chromeDatabasePaths)
            {
                DataTable historyDt = SqliteUtils.QueryDataTable(dbPath,new SQLiteCommand(query));

                foreach (DataRow row in historyDt.Rows)
                {
                    Uri uri;
                    DateTime lastVisit;
                    string title;
                    int? visitCount;

                    lastVisit = DateTime.Parse(row["last_visit"].ToString()).ToLocalTime();
                    if (!DateUtils.IsEntryInTimelimit(lastVisit, startTime, endTime))
                        continue;

                    try
                    {
                        uri = new Uri(row["url"].ToString(), UriKind.Absolute);
                    }
                    catch (UriFormatException)
                    {
                        continue;
                    }

                    title = row["title"].ToString();
                    title = string.IsNullOrEmpty(title)
                        ? null
                        : title;

                    visitCount = int.TryParse(row["visit_count"].ToString(), out int outVal)
                        ? (int?)outVal
                        : null;

                    HistoryEntry entry = new HistoryEntry(uri, title, lastVisit, visitCount, Browser.Chrome);
                    entryList.Add(entry);
                }
            }

            return entryList;
        }


        private IEnumerable<string> GetChromeDatabasePaths()
        {
            ICollection<string> databasePaths = new List<string>();
            string path = null;

            if (TryGetFullPathByProfile(DEFAULT_PROFILE_NAME, out path))
                databasePaths.Add(path);

            foreach (string userPath in Directory.EnumerateDirectories(FullDataPath, CUSTOM_PROFILE_PATTERN))
            {
                if (TryGetFullPathByProfile(new DirectoryInfo(userPath).Name, out path))
                    databasePaths.Add(path);
            }

            return databasePaths;
        }


        private bool TryGetFullPathByProfile(string profileName, out string fullPath)
        {
            string dbPath = Path.Combine(
                FullDataPath,
                profileName,
                DATABASE_NAME);

            fullPath = File.Exists(dbPath)
                ? dbPath
                : null;

            return fullPath == null
                ? false 
                : true;
        }
    }
}