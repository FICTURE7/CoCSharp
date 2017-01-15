using System;

namespace CoCSharp
{
    internal class TimeUtils
    {
        public const double TickDuration = (1d / 60d) * 1000d;

        private static readonly DateTime UnixTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static int UnixUtcNow
        {
            get { return (int)ToUnixTimestamp(DateTime.UtcNow); }
        }

        public static DateTime FromUnixTimestamp(double unixTimestamp)
        {
            return UnixTime.AddSeconds(unixTimestamp);
        }

        public static double ToUnixTimestamp(DateTime time)
        {
            return (time - UnixTime).TotalSeconds;
        }

        public static DateTime FromJavaTimestamp(double javaTimestamp)
        {
            return UnixTime.AddSeconds(javaTimestamp / 1000);
        }

        public static double ToJavaTimestamp(DateTime time)
        {
            return (time - UnixTime).TotalSeconds * 1000;
        }

        public static int ToTick(TimeSpan duration)
        {
            return (int)(duration.TotalMilliseconds / TickDuration);
        }

        public static int ToTick(int duration)
        {
            return (int)(duration * 1000d / TickDuration);
        }

        public static double FromTick(int tick)
        {
            return ((tick * TickDuration) / 1000d);
        }
    }
}
