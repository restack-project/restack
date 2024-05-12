namespace ReStack.Web.Extensions;

public static class DateTimeExtensions
{
    public static string ToDuration(this TimeSpan timeSpan)
    {
        var format = string.Empty;

        if (timeSpan.Hours > 0)
            format += "h\\h\\ ";
        if (timeSpan.Minutes > 0)
            format += "m\\m\\ ";

        if (timeSpan.TotalSeconds > 1)
            format += "s\\s";
        else
            return "< 1s";

        return timeSpan.ToString(format);
    }

    public static string ToAgo(this DateTime date)
    {
        if (date.Date == DateTime.UtcNow.Date)
        {
            var timeSpan = DateTime.UtcNow - date;
            if (timeSpan < TimeSpan.FromMinutes(1))
                return $"{timeSpan.Seconds}s ago";
            else if (timeSpan < TimeSpan.FromHours(1))
                return $"{timeSpan.Minutes}m ago";
            else
                return $"{timeSpan.Hours}h ago";
        }
        else if (date.Date == DateTime.UtcNow.AddDays(-1).Date)
        {
            return date.ToString("'Yesterday'");
        }
        else if (date.Date >= DateTime.UtcNow.AddDays(-7).Date)
        {
            return date.ToString("dddd");
        }
        else
        {
            return date.ToString("MMM dd");
        }
    }
}
