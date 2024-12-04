
internal partial class Day4
{
	internal static int TestXMAS(int X, int Y)
	{
		if (Input[Y][X] != 'X')
			return 0;

		int Occurences = 0;

		Occurences += SearchXMASDiagUpLeft(X, Y) ? 1 : 0;
		Occurences += SearchXMASDiagUpRight(X, Y) ? 1 : 0;
		Occurences += SearchXMASDiagDownLeft(X, Y) ? 1 : 0;
		Occurences += SearchXMASDiagDownRight(X, Y) ? 1 : 0;
		Occurences += SearchXMASUp(X, Y) ? 1 : 0;
		Occurences += SearchXMASDown(X, Y) ? 1 : 0;
		Occurences += SearchXMASLeft(X, Y) ? 1 : 0;
		Occurences += SearchXMASRight(X, Y) ? 1 : 0;

		return Occurences;
	}

	#region Diagonal Checks
	private static bool SearchXMASDiagUpLeft(int X, int Y)
	{
		if (X < 3 || Y < 3)
			return false;

		return
			Input[Y - 0][X - 0] == 'X' &&
			Input[Y - 1][X - 1] == 'M' &&
			Input[Y - 2][X - 2] == 'A' &&
			Input[Y - 3][X - 3] == 'S';
	}

	private static bool SearchXMASDiagDownLeft(int X, int Y)
	{
		if (X < 3 || Y > Height - 4)
			return false;

		return
			Input[Y + 0][X - 0] == 'X' &&
			Input[Y + 1][X - 1] == 'M' &&
			Input[Y + 2][X - 2] == 'A' &&
			Input[Y + 3][X - 3] == 'S';
	}

	private static bool SearchXMASDiagUpRight(int X, int Y)
	{
		if (X > Width - 4 || Y < 3)
			return false;

		return
			Input[Y - 0][X + 0] == 'X' &&
			Input[Y - 1][X + 1] == 'M' &&
			Input[Y - 2][X + 2] == 'A' &&
			Input[Y - 3][X + 3] == 'S';
	}

	private static bool SearchXMASDiagDownRight(int X, int Y)
	{
		if (X > Width - 4 || Y > Height - 4)
			return false;

		return
			Input[Y + 0][X + 0] == 'X' &&
			Input[Y + 1][X + 1] == 'M' &&
			Input[Y + 2][X + 2] == 'A' &&
			Input[Y + 3][X + 3] == 'S';
	}
	#endregion

	#region Vertical Checks
	private static bool SearchXMASUp(int X, int Y)
	{
		if (Y < 3)
			return false;

		return
			Input[Y - 0][X] == 'X' &&
			Input[Y - 1][X] == 'M' &&
			Input[Y - 2][X] == 'A' &&
			Input[Y - 3][X] == 'S';
	}

	private static bool SearchXMASDown(int X, int Y)
	{
		if (Y > Height - 4)
			return false;

		return
			Input[Y + 0][X] == 'X' &&
			Input[Y + 1][X] == 'M' &&
			Input[Y + 2][X] == 'A' &&
			Input[Y + 3][X] == 'S';
	}
	#endregion

	#region Horizontal Checks
	private static bool SearchXMASLeft(int X, int Y)
	{
		if (X < 3)
			return false;

		return
			Input[Y][X - 0] == 'X' &&
			Input[Y][X - 1] == 'M' &&
			Input[Y][X - 2] == 'A' &&
			Input[Y][X - 3] == 'S';
	}

	private static bool SearchXMASRight(int X, int Y)
	{
		if (X > Width - 4)
			return false;

		return
			Input[Y][X + 0] == 'X' &&
			Input[Y][X + 1] == 'M' &&
			Input[Y][X + 2] == 'A' &&
			Input[Y][X + 3] == 'S';
	}
	#endregion
}