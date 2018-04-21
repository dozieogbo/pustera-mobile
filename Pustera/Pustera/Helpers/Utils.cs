using System;

namespace Pustera.Helpers
{
    public class Utils
    {
        public static int HoursToSeconds(double hours)
        {
            return TimeSpan.FromHours(hours).Seconds;
        }
    }
}
