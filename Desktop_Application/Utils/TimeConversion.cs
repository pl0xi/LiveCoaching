using System;

namespace LiveCoaching.Utils;

public static class TimeConversion
{
    public static string CompareTimestampToCurrentTime(DateTime time)
    {
        var currentTime = DateTime.Now;

        // Year check
        var yearDifference = currentTime.Year - time.Year;
        if (yearDifference > 0) return $"{yearDifference} years ago";

        // Month check
        var monthDifference = currentTime.Month - time.Month;
        if (monthDifference > 0) return $"{monthDifference} months ago";

        // Day check 
        var dayDifference = currentTime.Day - time.Day;
        if (dayDifference > 0) return $"{dayDifference} days ago";

        // Hour check
        var hourDifference = currentTime.Hour - time.Hour;
        if (hourDifference > 0) return $"{hourDifference} hours ago";

        // Minute check 
        var minuteDifference = currentTime.Minute - time.Minute;
        if (minuteDifference > 0) return $"{minuteDifference} minutes ago";

        return "Just now";
    }
}