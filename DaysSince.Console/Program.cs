using System.IO;

namespace DaysSince.Console;

class Program
{
    static void Main(string[] args)
    {
        DateOnly now = DateOnly.FromDateTime(DateTime.Now.Date);
        WriteLine($"Today is {now}");

        ImmutableList<string> lines;
        try
        {
            lines = File.ReadAllLines("Dates.csv").ToImmutableList();
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
            invalidPairs.ForEach(i => WriteLine($"- {string.Join(";", i)}"));
        }

        var targetDates = pairs
            .Except(invalidPairs)
            .Select(p => new TargetDate(p[0], DateOnly.Parse(p[1])))
            .OrderByDescending(p => p.DaysSince)
            .ToImmutableList();

        targetDates.ForEach(d =>
        {
            Write($"• {d.Label}: Day #{d.DaysSince:#,##0}, counting from since {d.Date}");
            var milestone = new NextMilestone(1000, d);
            WriteLine($" ({milestone.DaysUntil:#,##0} on {milestone.Date})");
        });
    }
}
