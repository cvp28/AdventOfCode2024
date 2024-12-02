
public class Day2
{
    public static (int SafeReportCount, int DampenedSafeReportCount) Solution()
    {
        var Input = File.ReadAllLines(@".\Days\2\input.txt");
        List<int> CurrentLine = [];
        int SafeReportCount = 0;
        int DampenedSafeReportCount = 0;

        foreach (var line in Input)
        {
            CurrentLine.Clear();

            foreach (var c in line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                CurrentLine.Add(int.Parse(c));

            if (TestReport(CurrentLine))
            {
                SafeReportCount++;
                DampenedSafeReportCount++;
            }
            else
            {

                for (int i = 0; i < CurrentLine.Count; i++)
                {
                    List<int> CurrentLineDampened = new(CurrentLine);
                    CurrentLineDampened.RemoveAt(i);

                    if (TestReport(CurrentLineDampened))
                    {
                        DampenedSafeReportCount++;
                        break;
                    }   
                }
            }
        }

        return (SafeReportCount, DampenedSafeReportCount);
    }

    private static bool TestReport(List<int> Report)
    {
        int Difference = Math.Abs(Report[1] - Report[0]);

        if (Difference < 1 || Difference > 3)
            return false;

        bool IsDecreasing = Report[1] < Report[0];

        // Our input contains nothing but arrays that are at least > 3 elements, so just start at index 2
        for (int i = 2; i < Report.Count; i++)
        {
            bool TestDecreasing = Report[i] < Report[i - 1];

            if (IsDecreasing != TestDecreasing)
                return false;

            Difference = Math.Abs(Report[i] - Report[i - 1]);

            if (Difference < 1 || Difference > 3)
                return false;
        }

        return true;
    }
}