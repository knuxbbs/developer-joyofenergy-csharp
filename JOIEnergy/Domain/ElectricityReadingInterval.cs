using System;

namespace JOIEnergy.Domain;

public class ElectricityReadingInterval
{
    public readonly DateTime Start;
    public readonly DateTime End;

    private ElectricityReadingInterval(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static ElectricityReadingInterval GetPreviousWeekInterval()
    {
        return GetPreviousWeekInterval(DateTime.Now.Date);
    }

    public static ElectricityReadingInterval GetPreviousWeekInterval(DateTime date)
    {
        DateTime lastSunday = GetSunday(date.Date);
        DateTime sundayBeforeLast = lastSunday.AddDays(-7);

        return new ElectricityReadingInterval(sundayBeforeLast, lastSunday);
    }

    private static DateTime GetSunday(DateTime date)
    {
        while (date.DayOfWeek != DayOfWeek.Sunday)
        {
            date = date.AddDays(-1);
        }

        return date;
    }
}