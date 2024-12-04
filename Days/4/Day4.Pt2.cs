
internal partial class Day4
{
	internal static int TestMAS(int X, int Y)
	{
		if (Input[Y][X] != 'A')
			return 0;

		if (!IsValidPosition(X, Y))
			return 0;

		int Occurences = 0;

		// Here, the up/down/left/right refer to which side of the 'X' shape the two letter 'M's appear on
		// The X-MAS shape can only appear in one of 4 combinations
		Occurences += SearchMASUp(X, Y) ? 1 : 0;
		Occurences += SearchMASDown(X, Y) ? 1 : 0;
		Occurences += SearchMASLeft(X, Y) ? 1 : 0;
		Occurences += SearchMASRight(X, Y) ? 1 : 0;

		return Occurences;
	}

	private static bool IsValidPosition(int X, int Y) => X > 0 && X < Width - 1 && Y > 0 && Y < Height - 1;

	private static bool SearchMASUp(int X, int Y) =>
		Input[Y - 1][X - 1] == 'M' &&
		Input[Y - 1][X + 1] == 'M' &&
		Input[Y + 1][X - 1] == 'S' &&
		Input[Y + 1][X + 1] == 'S';

	private static bool SearchMASDown(int X, int Y) =>
		Input[Y - 1][X - 1] == 'S' &&
		Input[Y - 1][X + 1] == 'S' &&
		Input[Y + 1][X - 1] == 'M' &&
		Input[Y + 1][X + 1] == 'M';

	private static bool SearchMASLeft(int X, int Y) =>
		Input[Y - 1][X - 1] == 'M' &&
		Input[Y - 1][X + 1] == 'S' &&
		Input[Y + 1][X - 1] == 'M' &&
		Input[Y + 1][X + 1] == 'S';

	private static bool SearchMASRight(int X, int Y) =>
		Input[Y - 1][X - 1] == 'S' &&
		Input[Y - 1][X + 1] == 'M' &&
		Input[Y + 1][X - 1] == 'S' &&
		Input[Y + 1][X + 1] == 'M';
}