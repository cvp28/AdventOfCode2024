
public class Day1
{
	public static (int Difference, int Similarity) Solution()
	{
		List<int> List1 = [];
		List<int> List2 = [];
		int ListDifference = 0;
		int ListSimilarity = 0;

		var FileContents = File.ReadAllLines(@".\Days\1\input.txt");

		for (int i = 0; i < FileContents.Length; i++)
		{
			var CurrentLineElements = FileContents[i].Split([' ', '\n'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			List1.Add(int.Parse(CurrentLineElements[0]));
			List2.Add(int.Parse(CurrentLineElements[1]));
		}

		List1.Sort();
		List2.Sort();

		for (int i = 0; i < List1.Count; i++)
		{
			// Calculate Difference (Pt. 1)
			ListDifference += Math.Abs(List1[i] - List2[i]);

			// Calculate Similarity (Pt. 2)
			var Occurences = List2.Count(j => j == List1[i]);

			ListSimilarity += List1[i] * Occurences;
		}

		return (ListDifference, ListSimilarity);
	}
}