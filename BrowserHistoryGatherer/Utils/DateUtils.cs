using System;
using System.Globalization;

namespace BrowserHistoryGatherer.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateUtils
    {
        private static readonly DateTime DateTimeNewCentery = new DateTime(1970, 1, 1, 0, 0, 0);
        public static bool TryParsePlistToLocal(string dateString, out DateTime result)
        {
            result = DateTime.Parse("1/1/1");

            if (!double.TryParse(dateString, NumberStyles.Any, CultureInfo.InvariantCulture, out double msSince))
                return false;

            result = result.AddSeconds(msSince).ToLocalTime();
            return true;
        }

        public static bool IsEntryInTimelimit(DateTime entryVisit, DateTime? startTime, DateTime? endTime) {
            if (startTime == null && endTime == null)
                return true;

            bool isLaterThanStart = DateTime.Compare(entryVisit, (DateTime)startTime) >= 0;

            if (endTime == null)
                return isLaterThanStart;

            bool isEarlierThanEnd = DateTime.Compare(entryVisit, (DateTime)endTime) <= 0;

            if (isLaterThanStart && isEarlierThanEnd)
                return true;

            return false;
        }

        public static long GetTimeStampFromNewCentery(this DateTime dateTime) {
            return (long)(dateTime - DateTimeNewCentery).TotalSeconds;
        }
    }
}