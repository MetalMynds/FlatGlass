using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace MetalMynds.Utilities
{
    public static class DateTimeHelper
    {
        public static DateTime DatePlusYear(DateTime Start, int Years)
        {
            return Start.AddYears(Years).AddDays(-1);
        }

        public static DateTime FromJava(long TimeStamp)
        {

            DateTime baseDate = getBaseDate(true);

            return baseDate.AddMilliseconds(TimeStamp);

        }

        public static long ToJava(DateTime DateTime)
        {
            DateTime baseDate = getBaseDate(true);

            long timeInMillis = (DateTime.Ticks - baseDate.Ticks) / 10000;

            return timeInMillis;
        }

        public static String ToJavaString(DateTime DateTime)
        {
            return DateTime.ToString("dd-MM-yyyy");
        }

        public static DateTime FromJava(String JavaDateTime)
        {
            DateTime convertedDateTime = DateTime.MinValue;

            if (DateTime.TryParseExact(JavaDateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedDateTime))
            {
                return convertedDateTime;
            }
            else if (DateTime.TryParseExact(JavaDateTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out convertedDateTime))
            {
                return convertedDateTime;
            }
            else
            {
                throw new InvalidOperationException(String.Format("The Java Date [{0}] Can't be Converted to DateTime!", JavaDateTime));
            }

        }

        public static double ToTimeStamp(DateTime DateTime)
        {
            return ToTimeStamp(DateTime, false);
        }

        public static double ToTimeStamp(DateTime DateTime,Boolean UnixCompatible)
        {
            DateTime baseDate = getBaseDate(UnixCompatible);

            return (DateTime - baseDate).TotalMilliseconds;
        }

        public static DateTime FromTimeStamp(long TimeStamp)
        {
            return FromTimeStamp(TimeStamp, false);
        }

        public static DateTime FromTimeStamp(long TimeStamp, bool UnixCompatible)
        {
            DateTime baseDate = getBaseDate(UnixCompatible);

            return baseDate.AddMilliseconds(TimeStamp);

        }

        internal static DateTime getBaseDate(bool Unix)
        {
            if (Unix)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0); // DateTime.Parse("01/01/1970 00:00:00", CultureInfo.GetCultureInfo("en-GB"));
            }
            else
            {
                return DateTime.Parse("27/01/1974 12:00:00", CultureInfo.GetCultureInfo("en-GB"));
            }
        }

        public static class Now {
            
            public static long ToTimeStamp()
            {
                return ToTimeStamp(false);
            }
            
            public static long ToTimeStamp(Boolean UnixCompatible)
            {
                DateTime baseDate = getBaseDate(UnixCompatible);

                return DateTime.Now.Ticks - baseDate.Ticks;
            }

        }

        public static class UtcNow
        {
            public static long ToTimeStamp()
            {
                return ToTimeStamp(false);
            }

            public static long ToTimeStamp(Boolean UnixCompatible)
            {
                DateTime baseDate = getBaseDate(UnixCompatible);

                return DateTime.UtcNow.Ticks - baseDate.Ticks;
            }

        }

    }
}
