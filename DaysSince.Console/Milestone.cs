namespace DaysSince.Console;

/// <summary>
/// A marker of a certain number of days after an event.
/// </summary>
readonly struct Milestone
{
    /// <summary>
    /// The numbers that milestones should be a multiple of. (Example: `1000` means
    /// the 1,000th day and 2,000th day, and so on, should be considered milestones.)
    /// </summary>
    private static readonly int _interval = 1000;

    /// <summary>
    /// The calculated date of the milestone.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// The count of days between the original event date and the milestone date.
    /// </summary>
    public int DayCount { get; init; }

    /// <summary>
    /// The count of days between today's date and the milestone date.
    /// </summary>
    public int DaysUntil { get; init; }

    public Milestone(int daysSince)
    {
        DayCount = daysSince + _interval - (daysSince % _interval);
        DaysUntil = DayCount - daysSince;
        Date = DateOnly.FromDateTime(DateTime.Now.Date.AddDays(DaysUntil));
    }
}
