using System.IO;
using Spectre.Console;

namespace DaysSince.Console;

class Program
{
    private static readonly string _csvFileName = "Dates.csv";
    private static readonly int _milestoneInterval = 1000;

    static void Main()
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now.Date);
        WriteLine($"Today is {now}");

        ImmutableList<string> lines;
        try
        {
            lines = File.ReadAllLines(_csvFileName).ToImmutableList();
        }
        catch (Exception ex)
        {
            WriteLine($"Failure reading the date CSV file: {ex.Message}");
            return;
        }

        var pairs = lines.Select(l => l.Split(",")).ToImmutableList();

        var invalidPairs = pairs.Where(p => p.Length != 2).ToImmutableList();
        if (invalidPairs.Any())
        {
            WriteLine($"Warning: {invalidPairs.Count} invalid lines will be ignored");
            invalidPairs.ForEach(ip => WriteLine($"- {string.Join(";", ip)}"));
        }

        var targetDates = pairs
            .Except(invalidPairs)
            .Select(p => new TargetDate(p.First(), DateOnly.Parse(p.Last())))
            .OrderByDescending(p => p.DaysSince)
            .ToImmutableList();

        var table = new Table();
        table.AddColumn("Label");
        table.AddColumn(new TableColumn("Date").RightAligned());
        table.AddColumn(new TableColumn("Day No.").RightAligned());
        table.AddColumn("Next Milestone");
        targetDates.ForEach(d =>
        {
            NextMilestone milestone = new(_milestoneInterval, d);
            table.AddRow(
                d.Label,
                d.Date.ToString(),
                $"{d.DaysSince:#,##0}",
                $"{milestone.DaysUntil:#,##0} on {milestone.Date}");
        });
        AnsiConsole.Write(table);
    }
}
