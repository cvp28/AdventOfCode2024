
internal partial class Day4
{
	internal static string[] Input;

	internal static int Width;
	internal static int Height;

	public static (int Part1, int Part2) Solution()
	{
		Input = File.ReadAllLines(@".\Days\4\input.txt");

		Width = Input[0].Length;
		Height = Input.Length;

		int Pt1Occurences = 0;
		int Pt2Occurences = 0;

		for (int Y = 0; Y < Height; Y++)
			for (int X = 0; X < Width; X++)
			{
				Pt1Occurences += TestXMAS(X, Y);
				Pt2Occurences += TestMAS(X, Y);
			}

		return (Pt1Occurences, Pt2Occurences);

	}
}