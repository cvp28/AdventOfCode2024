
using System.Runtime.CompilerServices;

using Position = (int X, int Y);
//using PosDir = ((int X, int Y) Pos, Direction Dir);

internal class Day6
{
	private static HashSet<int> UniquePositionsVisited = [];
	//private static HashSet<int> PosDirHashes = [];

	private static int Height;
	private static int Width;

	public static (int Part1, int Part2) Solution()
	{
		var Input = File.ReadAllText(@".\Days\6\input.txt");

		Input.ReplaceLineEndings("\n");

		int Part1 = 0;
		int Part2 = 0;

		Width = Input.IndexOf('\n');
		Height = Input.Count(c => c == '\n');

		var Map = Input.ReplaceLineEndings(string.Empty).ToCharArray();

		Position StartPos = (0, 0);
		Direction StartDir = Direction.North;

		List<Position> BlankPositions = [];

		// Determine guard starting pos
		for (int i = 0; i < Height * Width; i++)
			switch (Map[i])
			{
				case '^':
					var Pos = ToPos(i);
					StartPos.X = Pos.X;
					StartPos.Y = Pos.Y;
					break;

				case '.':
					BlankPositions.Add(ToPos(i));
					break;
			}

		// Run sim once to compute Pt. 1
		Part1 = SimulatePatrolPt1(Map, StartPos, StartDir);


		// Run sim in a loop to compute Pt. 2

		// Each loop iteration, we replace one of the blank positions with an obstacle and re-run the sim to determine if that will end up looping
		// NOTE: This is not efficient in the slightest
		Parallel.For(0, BlankPositions.Count, delegate (int i)
		{
			Position Pos = BlankPositions[i];

			if (!SimulatePatrolPt2(Map, StartPos, StartDir, ToIndex(Pos)))
				Part2++;
		});

		//	for (int i = 0; i < BlankPositions.Count; i++)
		//	{
		//		Position Pos = BlankPositions[i];
		//	
		//		if (!SimulatePatrolPt2(Map, StartPos, StartDir, ToIndex(Pos)))
		//			Part2++;
		//	}

		return (Part1, Part2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int ToIndex(in Position Pos) => Pos.Y * Width + Pos.X;

	private static Position ToPos(int Index)
	{
		(int Y, int X) Pos = Math.DivRem(Index, Width);
		return (Pos.X, Pos.Y);
	}

	private static int SimulatePatrolPt1(char[] Map, Position Pos, Direction Dir)
	{
		UniquePositionsVisited.Clear();
		UniquePositionsVisited.Add(ToIndex(Pos));

		while (IsInLab(in Pos))
		{
			switch (CheckCanMoveForwardPt1(Map, Pos, in Dir))
			{
				case CheckResult.Collides:
					TurnRight(ref Dir);
					break;

				case CheckResult.Succeeds:
					MoveForward(ref Pos, in Dir);
					UniquePositionsVisited.Add(ToIndex(in Pos));
					break;

				case CheckResult.Exits:
					goto end;
			}
		}

	end:
		return UniquePositionsVisited.Count;
	}

	private static bool SimulatePatrolPt2(char[] Map, Position Pos, Direction Dir, int ObstacleIndex)
	{
		HashSet<int> PosDirHashes = [];

		while (IsInLab(in Pos))
		{
			// We check for looping by storing a list of hashes where each hash is computed from:
			// 1. The guard's current position
			// 2. The guard's current direction

			// A loop has happened if we ever end up in the same spot facing the same direction
			// Which is to say, the current sim iteration's hash is already in the list

			int Hash = HashCode.Combine(Pos, Dir);

			if (!PosDirHashes.Add(Hash))
			{
				// If we end up here, then the guard has done a loop
				return false;
			}

			switch (CheckCanMoveForwardPt2(Map, Pos, in Dir, in ObstacleIndex))
			{
				case CheckResult.Collides:
					TurnRight(ref Dir);
					break;

				case CheckResult.Succeeds:
					MoveForward(ref Pos, in Dir);
					break;

				case CheckResult.Exits:
					goto end;
			}
		}

	end:
		return true;
	}

	private static bool IsInLab(in Position Pos) =>
		Pos.X >= 0 &&
		Pos.X < Width &&
		Pos.Y >= 0 &&
		Pos.Y < Height;

	private static CheckResult CheckCanMoveForwardPt1(char[] Map, Position Pos, in Direction Dir)
	{
		MoveForward(ref Pos, in Dir);

		if (!IsInLab(in Pos))
			return CheckResult.Exits;

		return Map[ToIndex(Pos)] != '#' ? CheckResult.Succeeds : CheckResult.Collides;
	}

	private static CheckResult CheckCanMoveForwardPt2(char[] Map, Position Pos, in Direction Dir, in int ObstacleIndex)
	{
		MoveForward(ref Pos, in Dir);

		if (!IsInLab(in Pos))
			return CheckResult.Exits;

		int Index = ToIndex(Pos);

		return Map[Index] != '#' && Index != ObstacleIndex ? CheckResult.Succeeds : CheckResult.Collides;
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

//	struct GuardState
//	{
//		public int State;
//	
//		public byte X
//		{
//			get => (byte) State;
//			set => State = (byte) Dir << 16 | Y << 8 | value;
//		}
//	
//	
//		public byte Y
//		{
//			get => (byte) (State >> 8);
//			set => State = (byte) Dir << 16 | value << 8 | X;
//		}
//	
//		public Direction Dir
//		{
//			get => (Direction) (State >> 16);
//			set => State = (byte) value << 16 | Y << 8 | X;
//	
//		}
//	
//		public int AsIndex(int Width) => (Y * Width) + X;
//	
//	}

enum Direction : byte
{
	North,
	East,
	South,
	West
}

enum CheckResult : byte
{
	Exits,						// Guard leaves the lab
	Collides,					// Guard collides with an object '#'
	Succeeds					// Guard is able to move forward without issue
}