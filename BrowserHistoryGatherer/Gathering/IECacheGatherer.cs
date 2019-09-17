using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrowserHistoryGatherer.Data;

namespace BrowserHistoryGatherer.Gathering {
    public class IECacheGatherer : IBrowserCacheGatherer {
        /// <summary>
        /// Initializes a new instance of <see cref="IECacheGatherer"/>
        /// </summary>
        IECacheGatherer() {

        }
        public static IECacheGatherer Instance { get; } = new IECacheGatherer();

        public string BrowserName => Constants.BrowserName_IE;

        private static readonly string IEDataPath =
#if DEBUG
            "D:\\WebCache\\WebCache\\WebCacheV01.dat";
        //"D:\\WebCache\\Windows10FallDown.vhdx\\WebCache\\WebCacheV01.dat";
#else
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            +"\\Microsoft\\Windows\\WebCache\\WebCacheV01.dat";
#endif



        private const int ColumnIndex_Uri = 23;
        private const int ColumnIndex_Size = 11;
        private const int ColumnIndex_VisitTime = 1;
        private const int ColumnIndex_VisitCount = 0;

        private const string ContainerPrefix = "Container_";
        private const string HttpPrefix = "http";
        private static readonly DateTime NTStartDateTime = new DateTime(1601, 1, 1, 0, 0, 0);
        private const long TimeToMillisecondDevideCount = 10000;

        public ICollection<CacheEntry> GetBrowserCaches() {
            var caches = new List<CacheEntry>();
            if (!TryCopyDataToTempPath(out var tempPath)) {
                return caches;
            }

            DBReader dbReader = null;
            try {
                dbReader = new DBReader(tempPath);
                dbReader.Init(true);
                var allrows = dbReader.Tables.
                    Where(p => p.StartsWith(ContainerPrefix)).
                    SelectMany(p => dbReader.GetRows(p));
#if DEBUG
                //var groups = dbReader.Tables.SelectMany(p => dbReader.GetRows(p).Select(q => new { row = q, table = p })).GroupBy(p => {
                //    if(p.row.Count <= ColumnIndex_Uri) {
                //        return null;
                //    }
                //    if(p.row[ColumnIndex_Uri] == null) {
                //        return null;
                //    }

                //    return p.row[ColumnIndex_Uri]?.ToString().Substring(0, 6);
                //});
#endif
                foreach (var row in allrows) {
                    if (row.Count < ColumnIndex_Uri) {
                        continue;
                    }

                    if (!(row[ColumnIndex_Uri] is string uriString)) {
                        continue;
                    }
                    
                    if (!(row[ColumnIndex_VisitTime] is long visitTimeLong)) {
                        continue;
                    }

                    if (!(row[ColumnIndex_VisitCount] is uint visitCount)) {
                        continue;
                    }

                    if (!(row[ColumnIndex_Size] is long size)) {
                        continue;
                    }

                    if (!uriString.StartsWith(HttpPrefix)) {
                        continue;
                    }

                    var visitTime = NTStartDateTime.AddMilliseconds(visitTimeLong / TimeToMillisecondDevideCount);
                    var uri = new Uri(uriString);

                    var cache = new CacheEntry(uri, visitTime, visitCount,size);

                    caches.Add(cache);
                }

                return caches;
            }
            catch(Exception ex) {
                dbReader?.Dispose();
                Logger.WriteLine(ex.Message);
                Logger.WriteLine(ex.StackTrace);
                return caches;
            }

            
        }


        /// <summary>
        /// 拷贝数据文件至一个临时路径;
        /// </summary>
        /// <returns></returns>
        private bool TryCopyDataToTempPath(out string tempPath) {
            tempPath = null;
            if (!File.Exists(IEDataPath)) {
                return false;
            }

            try {
                tempPath = $"{Path.GetTempPath()}\\{Path.GetRandomFileName()}";

                File.Copy(IEDataPath, tempPath);
                return true;
            }
            catch(Exception ex) {
                return false;
            }
        }
    }
}
