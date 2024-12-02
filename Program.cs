
public class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0) return;
        if (!int.TryParse(args[0], out int Day)) return;

        Console.WriteLine(Day switch
        {
            1 => Day1.Solution(),
            2 => Day2.Solution(),

            _ => string.Empty
        });
    }
}