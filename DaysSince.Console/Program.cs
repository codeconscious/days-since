using System.IO;
using Spectre.Console;

namespace DaysSince.Console;

class Program
{
    private static readonly string _csvFileName = "Dates.csv";
    private static readonly int _milestoneInterval = 1000;
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
        DateOnly now = DateOnly.FromDateTime(DateTime.Now.Date);
        AnsiConsole.WriteLine($"Today is {now}");

        ImmutableList<string> lines;
        try
        {
            lines = File.ReadAllLines(_csvFileName).ToImmutableList();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Failure reading the date CSV file: {ex.Message}[/]");
            return;
        }

        var datePairs = lines.Select(l => l.Split(",")).ToImmutableList();
        var invalidPairs = datePairs.Where(_isNotValidPair).ToImmutableList();
        var targetDates = datePairs
            .Except(invalidPairs)
            .Select(p => new TargetDate(p.First(),
                                        DateOnly.Parse(p.Last())))
            .OrderByDescending(p => p.DaysSince)
            .ToImmutableList();

        PrintInvalidPairs(invalidPairs);
        PrintResults(targetDates);
    }

    static void PrintInvalidPairs(ImmutableList<string[]> invalidPairs)
    {
        if (invalidPairs?.Any() != true)
            return;

        AnsiConsole.MarkupLine($"[yellow]Warning: {invalidPairs.Count} invalid line(s) will be ignored:[/]");
        invalidPairs.ForEach(invalidPair =>
            AnsiConsole.WriteLine($"- {string.Join(";", invalidPair)}"));
    }

    static void PrintResults(ImmutableList<TargetDate> targetDates)
    {
        var table = new Table();
        table.AddColumn("Label");
        table.AddColumn(new TableColumn("Date").RightAligned());
        table.AddColumn(new TableColumn("Day No.").RightAligned());
        table.AddColumn("Next Milestone");

        targetDates.ForEach(date =>
        {
            NextMilestone milestone = new(_milestoneInterval, date);
            table.AddRow(
                date.Label,
                date.Date.ToString(),
                $"{date.DaysSince:#,##0}",
                $"{milestone.DaysUntil:#,##0} on {milestone.Date}");
        });
        AnsiConsole.Write(table);
    }
}
