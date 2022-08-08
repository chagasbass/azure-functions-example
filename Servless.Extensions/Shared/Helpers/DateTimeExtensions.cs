namespace Serverless.Extensions.Shared.Helpers;

public static class DateTimeExtensions
{
    public static DateTime GetGmtDateTime(this DateTime data) => data.AddHours(-3);
    public static DateTime? GetGmtDateTime(this DateTime? data) => data.Value.AddHours(-3);
}
