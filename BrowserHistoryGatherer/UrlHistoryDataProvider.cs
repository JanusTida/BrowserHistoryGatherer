using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FILETIME = System.Runtime.InteropServices.ComTypes.FILETIME;

namespace BrowserHistoryGatherer {
    public sealed class UrlHistoryDataProvider {
        private List<string> _urlHistory;

        public IEnumerable<string> GetSuggestions(string query) {
            if (_urlHistory == null) {
                lock (this) {
                    _urlHistory = LoadHistoryUrls();
                }
            }

            //if("http://".IndexOf(query, StringComparison.Ordinal) == 0)
            //{
            //    return _urlHistory;
            //}

            var pattern = query;
            if (!pattern.StartsWith("http://"))
                pattern = "http://" + query;

            var suggestions = _urlHistory.Where(url => url.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)).ToList();

            if (!query.StartsWith("http://") && !query.StartsWith("www.")) {
                var s = _urlHistory.Where(url => url.StartsWith(pattern, StringComparison.OrdinalIgnoreCase));
                suggestions.AddRange(s);
            }
            return suggestions;
        }

        public string GetAppendText(string query, string firstMatch) {
            var pattern = query;
            if (!pattern.StartsWith("http://"))
                pattern = "http://" + pattern;

            if (firstMatch.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) != 0)
                return null;

            var result = firstMatch.Substring(query.Length);
            var slashPos = result.IndexOf('/');
            if (slashPos != -1)
                result = result.Substring(0, slashPos + 1);

            return result;
        }

        #region Native Methods

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr FindFirstUrlCacheEntry(string lpszUrlSearchPattern,
                                                           IntPtr lpFirstCacheEntryInfo,
                                                           ref int lpdwFirstCacheEntryInfoBufferSize);
        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool FindNextUrlCacheEntry(IntPtr hEnumHandle,
                                                        IntPtr lpNextCacheEntryInfo,
                                                        ref int lpdwNextCacheEntryInfoBufferSize);

        [DllImport("wininet.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern long FindCloseUrlCache(IntPtr hEnumHandle);

        private const int ErrorNoMoreItems = 0x103;
        private const int ErrorInsufficientBuffer = 122;

        public static List<string> LoadHistoryUrls() {
            var result = new List<string>();
            int cb = 0;
            const string pattern = "visited:";
            FindFirstUrlCacheEntry(pattern, IntPtr.Zero, ref cb);
            var sz = Marshal.SizeOf(typeof(_INTERNET_CACHE_ENTRY_INFO));
            
            IntPtr buf = Marshal.AllocHGlobal(cb);
            try {
                IntPtr hFind = FindFirstUrlCacheEntry(pattern, buf, ref cb);
                while (true) {
                    var st = Marshal.PtrToStructure<_INTERNET_CACHE_ENTRY_INFO>(buf);
                    var pSourceUrl = Marshal.ReadIntPtr(buf, 4);
                    var url = Marshal.PtrToStringAnsi(pSourceUrl);
                    Debug.Assert(url != null, "url != null");
                    int atPos = url.IndexOf("@", StringComparison.Ordinal);
                    if (atPos != -1) {
                        url = url.Substring(atPos + 1);
                    }
                    if (url.StartsWith("http://")) {
                        result.Add(url);
                    }

                    bool retval = FindNextUrlCacheEntry(hFind, buf, ref cb);
                    if (!retval) {
                        var win32Err = Marshal.GetLastWin32Error();
                        if (win32Err == ErrorNoMoreItems) {
                            break;
                        }
                        if (win32Err == ErrorInsufficientBuffer) {
                            buf = Marshal.ReAllocHGlobal(buf, new IntPtr(cb));
                            FindNextUrlCacheEntry(hFind, buf, ref cb);
                        }
                    }
                }
                FindCloseUrlCache(hFind);
            }
            finally {
                Marshal.FreeHGlobal(buf);
            }
            return result;
        }

        #endregion
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct _INTERNET_CACHE_ENTRY_INFO {
        public uint dwStructSize;
        public string lpszSourceUrlName;
        public string lpszLocalFileName;
        public uint CacheEntryType;
        public uint dwUseCount;
        public uint dwHitRate;
        public uint dwSizeLow;
        public uint dwSizeHigh;
        public FILETIME LastModifiedTime;
        public FILETIME ExpireTime;
        public FILETIME LastAccessTime;
        public FILETIME LastSyncTime;
        public IntPtr lpHeaderInfo;
        public uint dwHeaderInfoSize;
        public string lpszFileExtension;
        public uint dwReserved;
    }
}
