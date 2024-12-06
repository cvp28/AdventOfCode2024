
using Position = (int X, int Y);
using LabMap = char[][];

internal class Day6
{
	private static HashSet<Position> UniquePositionsVisited = [];
	private static HashSet<int> PosDirHashes = [];

	private static int Height;
	private static int Width;

	public static (int Part1, int Part2) Solution()
	{
		var Input = File.ReadAllLines(@".\Days\6\input.txt").Select(s => s.ToCharArray()).ToArray();

		int Part1 = 0;
		int Part2 = 0;

		Height = Input.Length;
		Width = Input[0].Length;

		Position GuardStartPos = (0, 0);
		Direction GuardStartDir = Direction.North;

		List<Position> BlankPositions = [];

		// Determine guard starting pos
		for (int y = 0; y < Height; y++)
			for (int x = 0; x < Width; x++)
				switch (Input[y][x])
				{
					case '^':
						GuardStartPos.X = x;
						GuardStartPos.Y = y;
						break;

					case '.':
						BlankPositions.Add((x, y));
						break;
				}

		// Run sim once to compute Pt. 1
		SimulatePatrol(Input, GuardStartPos, GuardStartDir, out Part1);


		// Run sim in a loop to compute Pt. 2

		// Each loop iteration, we replace one of the blank positions with an obstacle and re-run the sim to determine if that will end up looping
		// NOTE: This is not efficient in the slightest
		for (int i = 0; i < BlankPositions.Count; i++)
		{
			Position Pos = BlankPositions[i];

			Input[Pos.Y][Pos.X] = '#';

			if (!SimulatePatrol(Input, GuardStartPos, GuardStartDir, out _))
				Part2++;

			Input[Pos.Y][Pos.X] = '.';
		}

		return (Part1, Part2);
	}

	private static bool SimulatePatrol(LabMap Map, Position Pos, Direction Dir, out int UniquePosCount)
	{
		UniquePositionsVisited.Clear();
		PosDirHashes.Clear();

		UniquePositionsVisited.Add(Pos);

		while (IsInLab(Map, Pos))
		{
			// We check for looping by storing a list of hashes where each hash is computed from:
			// 1. The guard's current position
			// 2. The guard's current direction

			// A loop has happened if we ever end up in the same spot facing the same direction
			// Which is to say, the current sim iteration's hash is already in the list

			var PosDirHash = HashCode.Combine(Pos, Dir);

			if (!PosDirHashes.Add(PosDirHash))
			{
				// If we end up here, then the guard has done a loop
				UniquePosCount = UniquePositionsVisited.Count;
				return false;
			}

			switch (CheckCanMoveForward(Map, Pos, Dir, PosDirHashes))
			{
				case CheckResult.Collides:
					TurnRight(ref Dir);
					break;

				case CheckResult.Succeeds:
					MoveForward(ref Pos, Dir);
					UniquePositionsVisited.Add(Pos);
					break;

				case CheckResult.Exits:
					goto end;
			}
		}

	end:
		UniquePosCount = UniquePositionsVisited.Count;
		return true;
	}

	private static bool IsInLab(LabMap Map, Position Pos) =>
		Pos.X >= 0 &&
		Pos.X < Width &&
		Pos.Y >= 0 &&
		Pos.Y < Height;

	private static CheckResult CheckCanMoveForward(LabMap Map, Position Pos, Direction Dir, HashSet<int> PosDirHashes)
	{
		Position CheckPos = Pos;
		MoveForward(ref CheckPos, Dir);

		if (!IsInLab(Map, CheckPos))
			return CheckResult.Exits;

		return Map[CheckPos.Y][CheckPos.X] != '#' ? CheckResult.Succeeds : CheckResult.Collides;
	}

	private static void MoveForward(ref Position Pos, Direction Dir)
	{
		switch (Dir)
		{
			case Direction.North:	Pos = (Pos.X, --Pos.Y); break;
			case Direction.East:	Pos = (++Pos.X, Pos.Y); break;
			case Direction.South:	Pos = (Pos.X, ++Pos.Y); break;
			case Direction.West:	Pos = (--Pos.X, Pos.Y); break;
		}
	}

	private static void TurnRight(ref Direction Dir)
	{
		switch (Dir)
		{
			case Direction.North:	Dir = Direction.East; break;
			case Direction.East:	Dir = Direction.South; break;
			case Direction.South:	Dir = Direction.West; break;
			case Direction.West:	Dir = Direction.North; break;
		}
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
	Succeeds					// Guard is able to move forward without issue (and will never loop)
}