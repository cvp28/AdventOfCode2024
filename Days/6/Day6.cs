
using System.Runtime.CompilerServices;

using Position = (int X, int Y);
using PosDir = ((int X, int Y) Pos, Direction Dir);

internal class Day6
{
	private static HashSet<Position> UniquePositionsVisited = [];
	private static HashSet<PosDir> PosDirHashes = [];

	private static int Height;
	private static int Width;

	public static (int Part1, int Part2) Solution()
	{
		var Input = File.ReadAllText(@".\Days\6\input.txt");

		int Part1 = 0;
		int Part2 = 0;

		Width = Input.IndexOf('\n');
		Height = Input.Count(c => c == '\n');

		var Map = Input.ReplaceLineEndings(string.Empty).ToCharArray();

		Position GuardStartPos = (0, 0);
		Direction GuardStartDir = Direction.North;

		List<Position> BlankPositions = [];

		// Determine guard starting pos
		for (int i = 0; i < Height * Width; i++)
			switch (Map[i])
			{
				case '^':
					GuardStartPos = ToPos(i);
					break;

				case '.':
					BlankPositions.Add(ToPos(i));
					break;
			}

		// Run sim once to compute Pt. 1
		SimulatePatrol(Map, GuardStartPos, GuardStartDir, out Part1);


		// Run sim in a loop to compute Pt. 2

		// Each loop iteration, we replace one of the blank positions with an obstacle and re-run the sim to determine if that will end up looping
		// NOTE: This is not efficient in the slightest
		for (int i = 0; i < BlankPositions.Count; i++)
		{
			Position Pos = BlankPositions[i];

			Map[ToIndex(in Pos)] = '#';

			if (!SimulatePatrol(Map, GuardStartPos, GuardStartDir, out _))
				Part2++;

			Map[ToIndex(in Pos)] = '.';
		}

		return (Part1, Part2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int ToIndex(in Position Pos) => Pos.Y * Width + Pos.X;

	private static Position ToPos(int Index)
	{
		(int Y, int X) Pos = Math.DivRem(Index, Width);
		return (Pos.X, Pos.Y);
	}

	private static bool SimulatePatrol(char[] Map, Position Pos, Direction Dir, out int UniquePosCount)
	{
		UniquePositionsVisited.Clear();
		PosDirHashes.Clear();

		UniquePositionsVisited.Add(Pos);

		while (IsInLab(in Pos))
		{
			// We check for looping by storing a list of hashes where each hash is computed from:
			// 1. The guard's current position
			// 2. The guard's current direction

			// A loop has happened if we ever end up in the same spot facing the same direction
			// Which is to say, the current sim iteration's hash is already in the list

			if (!PosDirHashes.Add((Pos, Dir)))
			{
				// If we end up here, then the guard has done a loop
				UniquePosCount = UniquePositionsVisited.Count;
				return false;
			}

			switch (CheckCanMoveForward(Map, in Pos, in Dir))
			{
				case CheckResult.Collides:
					TurnRight(ref Dir);
					break;

				case CheckResult.Succeeds:
					MoveForward(ref Pos, in Dir);
					UniquePositionsVisited.Add(Pos);
					break;

				case CheckResult.Exits:
					goto end;
			}

			// Draw Map for debugging
			Console.ReadKey(true);
			Console.Clear();
			for (int y = Pos.Y - 20; y < Pos.Y + 20; y++)
			{
				for (int x = Pos.X - 20; x < Pos.X + 20; x++)
				{
					if ((x, y) == Pos)
					{
						Console.ForegroundColor = ConsoleColor.Black;
						Console.BackgroundColor = ConsoleColor.Red;
						Console.Write(Dir switch
						{
							Direction.North => '^',
							Direction.East => '>',
							Direction.South => 'v',
							Direction.West => '<',
						});
						Console.ResetColor();
						continue;
					}

					char MapChar = Map[ToIndex((x, y))];
			
					switch (MapChar)
					{
						case '^':
						case '.':
							Console.Write(' ');
							break;
			
						case '#':
							Console.Write('#');
							break;
					}
				}
			
			    Console.WriteLine();
			}
			Console.WriteLine(Pos);
		}

	end:
		UniquePosCount = UniquePositionsVisited.Count;
		return true;
	}

	private static bool IsInLab(in Position Pos) =>
		Pos.X >= 0 &&
		Pos.X < Width &&
		Pos.Y >= 0 &&
		Pos.Y < Height;

	private static CheckResult CheckCanMoveForward(char[] Map, in Position Pos, in Direction Dir)
	{
		Position CheckPos = Pos;
		MoveForward(ref CheckPos, in Dir);

		if (!IsInLab(in CheckPos))
			return CheckResult.Exits;

		return Map[ToIndex(in Pos)] != '#' ? CheckResult.Succeeds : CheckResult.Collides;
	}

	private static void MoveForward(ref Position Pos, in Direction Dir)
	{
		switch (Dir)
		{
			case Direction.North:	Pos.Y--; break;
			case Direction.East:	Pos.X++; break;
			case Direction.South:	Pos.Y++; break;
			case Direction.West:	Pos.X--; break;
		}
	}

	private static void TurnRight(ref Direction Dir)
	{
		Dir = Dir switch
		{
			Direction.North =>	Direction.East,
			Direction.East =>	Direction.South,
			Direction.South =>	Direction.West,
			Direction.West =>	Direction.North
		};
	}
}

enum Direction
{
	North,
	East,
	South,
	West
}

enum CheckResult
{
	Exits,						// Guard leaves the lab
	Collides,					// Guard collides with an object '#'
	Succeeds					// Guard is able to move forward without issue
}