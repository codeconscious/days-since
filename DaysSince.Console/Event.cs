namespace DaysSince.Console;

/// <summary>
/// An event that has occurred in the past.
/// </summary>
readonly struct Event
{
    public static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Now);

    /// <summary>
    /// An arbitrary name to identify this event.
    /// </summary>
    public string Label { get; init; }

    /// <summary>
    /// The date of the specified event.
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// Days between the event date and today's date.
    /// </summary>
    public int DaysSince { get; init; }

    /// <summary>
    /// The next calculated milestone.
    /// </summary>
    public Milestone Milestone { get; init; }

    public Event(string label, DateOnly date)
    {
        Label = string.IsNullOrWhiteSpace(label)
            ? throw new ArgumentException("An invalid label was provided.")
            : label;
        Date = date;
        DaysSince = Today.DayNumber - date.DayNumber + 1;
        Milestone = new(DaysSince);
    }
}
