
using System.Text.RegularExpressions;

public partial class Day3
{
    [GeneratedRegex(@"mul\((?<1>\d*),(?<2>\d*)\)")]
    internal static partial Regex MultiplyExpression();

    [GeneratedRegex(@"do\(\)")]
    internal static partial Regex DoExpression();

    [GeneratedRegex(@"don't\(\)")]
    internal static partial Regex DontExpression();

    public static (int Pt1Total, int Pt2Total) Solution()
    {
        var Input = File.ReadAllText(@".\Days\3\input.txt");

        // Match all expressions that we're on the lookout for
        var MulMatches = MultiplyExpression().Matches(Input);
        var DoMatches = DoExpression().Matches(Input);
        var DontMatches = DontExpression().Matches(Input);

        // Gather all matches into one collection
        var AllMatches = Enumerable.Concat(MulMatches, DoMatches);
        AllMatches = AllMatches.Concat(DontMatches);

        // Sort matches by their found index in the original string
        var MatchList = AllMatches.ToList();

        MatchList.Sort((m1, m2) => m1.Index.CompareTo(m2.Index));

        int Pt1Total = 0;
        int Pt2Total = 0;
        var MultiplyEnabled = true;

        for (int i = 0; i < MatchList.Count; i++)
        {
            MultiplyEnabled = MatchList[i].Value switch
            {
                "don't()" => false,
                "do()" => true,

                _ => MultiplyEnabled
            };

            // Basically "if is a mul expression"
            if (MatchList[i].Groups.Count >= 2)
            {
                if (MultiplyEnabled)
                    Pt2Total += int.Parse(MatchList[i].Groups["1"].Value) * int.Parse(MatchList[i].Groups["2"].Value);

                Pt1Total += int.Parse(MatchList[i].Groups["1"].Value) * int.Parse(MatchList[i].Groups["2"].Value);
            }
        }

        return (Pt1Total, Pt2Total);
    }
}
