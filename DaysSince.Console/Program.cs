using System.IO;
using Spectre.Console;

namespace DaysSince.Console;

class Program
{
    private static readonly string _csvFileName = "dates.csv";
    private static readonly Func<string[], bool> _isNotValidPair = b => b.Length != 2;

    static void Main()
    {
        try
        {
            Run();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {ex.Message}[/]");
        }
    }

    static void Run()
    {
        ImmutableList<string> lines;
        try
        {
            lines = File.ReadAllLines(_csvFileName).ToImmutableList();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failure reading the CSV file: {ex.Message}[/]");
            return;
        }

        var datePairs = lines.Select(l => l.Split(",")).ToImmutableList();
        var invalidPairs = datePairs.Where(_isNotValidPair).ToImmutableList();
        var eventDates = datePairs.Except(invalidPairs)
                                  .Select(p => new Event(p.First(),
                                                         DateOnly.Parse(p.Last())))
                                  .OrderByDescending(p => p.DaysSince)
                                  .ToImmutableList();

        PrintInvalidPairs(invalidPairs);
        PrintResults(eventDates);
    }

    static void PrintInvalidPairs(ImmutableList<string[]> invalidPairs)
    {
        if (invalidPairs?.Any() != true)
            return;

        AnsiConsole.MarkupLine($"[yellow]Warning: {invalidPairs.Count} invalid line(s) will be ignored:[/]");
        invalidPairs.ForEach(invalidPair =>
            AnsiConsole.WriteLine($"- {string.Join(";", invalidPair)}"));
    }

    static void PrintResults(ImmutableList<Event> eventDates)
    {
        AnsiConsole.WriteLine($"Today is {Event.Today}");

        var table = new Table();
        table.AddColumn("Label");
        table.AddColumn(new TableColumn("Date").RightAligned());
        table.AddColumn(new TableColumn("Day No.").RightAligned());
        table.AddColumn("Next Milestone");

        eventDates.ForEach(date =>
        {
            table.AddRow(
                date.Label,
                date.Date.ToString(),
                $"{date.DaysSince:#,##0}",
                $"{date.Milestone.DayCount:#,##0} in {date.Milestone.DaysUntil} days on {date.Milestone.Date}");
        });
        AnsiConsole.Write(table);
    }
}
