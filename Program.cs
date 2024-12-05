
public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Day5.Solution());

        if (args.Length == 0) return;
        if (!int.TryParse(args[0], out int Day)) return;

        Console.WriteLine(Day switch
        {
            1 => Day1.Solution(),
            2 => Day2.Solution(),
            3 => Day3.Solution(),
            4 => Day4.Solution(),
            5 => Day5.Solution(),

            _ => string.Empty
        });
    }
}