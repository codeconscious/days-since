namespace DaysSince.Console;

readonly struct TargetDate
{
    private static readonly DateOnly Today = DateOnly.FromDateTime(DateTime.Now);

    public string Label { get; init; }
    public DateOnly Date { get; init; }
    public int DaysSince { get; init; }
    public NextMilestone MileStone { get; init; }

    public TargetDate(string label, DateOnly date)
    {
        Label = string.IsNullOrWhiteSpace(label)
            ? throw new ArgumentException("An invalid label was provided.")
            : label;
        Date = date;
        DaysSince = Today.DayNumber - date.DayNumber + 1;
    }
}
