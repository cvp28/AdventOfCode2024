
internal class Day5
{
	static Dictionary<int, List<int>> Rules = [];
	static List<List<int>> Updates = [];

	public static (int Pt1Count, int Pt2Count) Solution()
	{
		var Input = File.ReadAllLines(@".\Days\5\input.txt");
		var Pt1Count = 0;
		var Pt2Count = 0;

		// Construct list of rules
		var rs = Input.Where(l => l.Contains("|"));
		foreach (var r in rs)
		{
			var elems = r.Split('|');
			var key = int.Parse(elems[0]);
			var val = int.Parse(elems[1]);

			if (!Rules.ContainsKey(key))
				Rules[key] = [];

			Rules[key].Add(val);
        }

		// Construct list of updates
		var us = Input.Where(l => l.Contains(","));
		foreach (var u in us)
		{
			List<int> temp = [];

			var elems = u.Split(',');

			foreach (var elem in elems)
				temp.Add(int.Parse(elem));

			Updates.Add(temp);
		}

		foreach (var Update in Updates)
		{
			var MiddleIndex = Update.Count / 2;

			if (TestUpdate(Update))
			{
				Pt1Count += Update[MiddleIndex];
			}
			else
			{
				Update.Sort((i, j) => !Rules[i].Contains(j) ? 1 : -1);
				Pt2Count += Update[MiddleIndex];
			}
		 }

        return (Pt1Count, Pt2Count);
	}

	private static bool TestUpdate(List<int> Update)
	{
		bool IsValid = true;

		for (int i = 0; i < Update.Count; i++)
		{
			if (!Rules.TryGetValue(Update[i], out var Rule))
				continue;

			var ForwardCheck = Update[(i + 1) .. Update.Count];
			var BackwardCheck = Update[0 .. i];

			IsValid &= ForwardCheck.All(Value =>
			{
				if (!Rules.TryGetValue(Value, out var TestRule))
					return true;

				return !TestRule.Contains(Update[i]);
			});

			IsValid &= BackwardCheck.All(Value => !Rule.Contains(Value));
		}

		return IsValid;
	}
}