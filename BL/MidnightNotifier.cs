using Microsoft.Win32; //SystemEvents
using System;
using System.Timers; //Timer

namespace CurrencyConversion.BL
{
    //Taken from https://stackoverflow.com/questions/8480063/event-for-datechange-at-midnight?noredirect=1&lq=1
    static class MidnightNotifier
    {
        private static readonly Timer timer;

        static MidnightNotifier()
        {
            timer = new Timer(GetSleepTime());
            timer.Elapsed += (s, e) =>
            {
                OnDayChanged();
                timer.Interval = GetSleepTime();
            };
            timer.Start();

            SystemEvents.TimeChanged += OnSystemTimeChanged;
        }

        private static double GetSleepTime()
        {
           var midnightTonight = DateTime.Today.AddDays(1);
            var differenceInMilliseconds = (midnightTonight - DateTime.Now).TotalMilliseconds;
            return differenceInMilliseconds;
        }

        private static void OnDayChanged()
        {
            DayChanged?.Invoke(null, null);
        }

        private static void OnSystemTimeChanged(object sender, EventArgs e)
        {
            timer.Interval = GetSleepTime();
        }

        public static event EventHandler<EventArgs> DayChanged;
    }
}
