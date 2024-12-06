
using System.Diagnostics;

public class Program
{
    static void Main(string[] args)
    {
        var start = Stopwatch.GetTimestamp();
        var sol = Day6.Solution();
        var time_taken = Stopwatch.GetElapsedTime(start);

        Console.WriteLine($"Found solution '{sol}' in {time_taken.TotalMilliseconds}");

        Console.WriteLine();

        if (args.Length == 0) return;
        if (!int.TryParse(args[0], out int Day)) return;

        Console.WriteLine(Day switch
        {
            1 => Day1.Solution(),
            2 => Day2.Solution(),
            3 => Day3.Solution(),
            4 => Day4.Solution(),
            5 => Day5.Solution(),
            6 => Day6.Solution(),

            _ => string.Empty
        });
    }
}