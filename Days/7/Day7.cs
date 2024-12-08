
internal class Day7
{
	private static List<Calibration> Calibrations = [];

	public static (ulong Part1, ulong Part2) Solution()
	{
		var Input = File.ReadAllLines(@".\Days\7\input.txt");

		ulong Part1 = 0;
		ulong Part2 = 0;

		// Construct list of calibration equations
		foreach (var Line in Input)
		{
			var Elems = Line.Split([':'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

			var Result = ulong.Parse(Elems[0]);
			var Components = Elems[1].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(ulong.Parse).ToArray();

			var cal = new Calibration()
			{
				Result = Result,
				Components = Components
			};

			Calibrations.Add(cal);
		}

		// Oh yeah we about to brute force the shit out of this

		HashSet<ulong> Pt1Successes = [];

		// Pt. 1
		for (int i = 0; i < Calibrations.Count; i++)
		{
			int NumCombinations = Calibrations[i].TotalCombinationsPt1;
			
			for (int Combination = 0; Combination < NumCombinations; Combination++)
			{
				var Test = Calibrations[i].PerformCalculationPt1(Combination);

				if (Test == Calibrations[i].Result)
				{
					Part1 += Calibrations[i].Result;
					Part2 += Calibrations[i].Result;
					Pt1Successes.Add(Calibrations[i].Result);
					break;
				}
			}
		}

		// Pt. 2
		Parallel.For(0, Calibrations.Count, delegate (int i)
		{
			int NumCombinations = Calibrations[i].TotalCombinationsPt2;

			for (int Combination = 0; Combination < NumCombinations; Combination++)
			{
				var Test = Calibrations[i].PerformCalculationPt2(Combination);

				if (Test == Calibrations[i].Result)
				{
					if (!Pt1Successes.Contains(Calibrations[i].Result))
						Part2 += Calibrations[i].Result;

					break;
				}
			}
		});

		return (Part1, Part2);
	}
}

public struct Calibration
{
	public ulong Result;

	public ulong[] Components;

	public int TotalCombinationsPt1 => (int) Math.Pow(2, Components.Length - 1);
	public int TotalCombinationsPt2 => (int) Math.Pow(3, Components.Length - 1);

	public ulong PerformCalculationPt1(int Combination)
	{
		ulong Result = Components[0];

		for (int i = 0; i < Components.Length - 1; i++)
		{
			char Op = GetBit(Combination, i);

			var Left = Result;
			var Right = Components[i + 1];

			Result = Op switch
			{
				'0' => Left + Right,
				'1' => Left * Right
			};
		}

		return Result;
	}

	public ulong PerformCalculationPt2(int Combination)
	{
		ulong Result = Components[0];

		for (int i = 0; i < Components.Length - 1; i++)
		{
			char Op = GetBase3Bit(Combination, i);

			var Left = Result;
			var Right = Components[i + 1];

			Result = Op switch
			{
				'0' => Left + Right,
				'1' => Left * Right,
				'2' => ulong.Parse(Left.ToString() + Right.ToString())
			};
		}

		return Result;
	}

	private char GetBit(int b, int pos) => ((b >> pos) & 1) != 0 ? '1' : '0';

	private char GetBase3Bit(int Num, int Pos)
	{
		int r = 0;

		for (int i = 0; i < Pos + 1; i++)
		{
			r = Num % 3;
			Num /= 3;
		}

		return r switch
		{
			0 => '0',
			1 => '1',
			2 => '2'
		};
	}
}