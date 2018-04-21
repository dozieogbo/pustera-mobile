using Pustera.Helpers;
using System;

namespace Pustera
{
    [Preserve(AllMembers = true)]
    public class Notification
    {
        public string Title { get; set; }
        public string Photo { get; set; }
        public string Url { get; set; }
        public long Time { get; set; }
        public DateTime Date => new DateTime(1970, 1, 1).AddMilliseconds(Time * 1000).ToUniversalTime();
    }
}
