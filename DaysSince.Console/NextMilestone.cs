namespace DaysSince.Console;

readonly struct NextMilestone
{
    public DateOnly Date { get; init; }
    public int DaysUntil { get; init; }

    public NextMilestone(int multipleOf, TargetDate originalDate)
    {
        int daysUntil = originalDate.DaysSince + multipleOf - originalDate.DaysSince % multipleOf;
        DaysUntil = daysUntil;

        int diff = daysUntil - originalDate.DaysSince;
        Date = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(diff));
    }
}
