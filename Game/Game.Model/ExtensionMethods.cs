using System;

namespace Game.Model
{
    public static class ExtensionMethods
    {
        public static DateTime IncrementMonths(this DateTime time, int months)
        {
            int month = time.Month;
            int year = time.Year;

            for (int i = 0; i < months; ++i)
            {
                if (month == 12)
                {
                    year++;
                    month = 1;
                }
                else
                    ++month;
            }

            return new DateTime(year, month, time.Day, time.Hour, time.Minute, time.Second);
        }
    }
}
