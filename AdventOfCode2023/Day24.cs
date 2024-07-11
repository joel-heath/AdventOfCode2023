namespace AdventOfCode2023;
public class Day24 : IDay
{
    public int Day => 24;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3", "2" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "19, 13, 30 @ -2,  1, -2\r\n18, 19, 22 @ -1, -1, -2\r\n20, 25, 34 @ -2, -2, -4\r\n12, 31, 28 @ -1, -2, -1\r\n20, 19, 15 @  1, -5, -3", "47" }
    };

    public string SolvePart1(string input)
    {
        long total = 0;

        var allNums = Utils.GetLongs(input).ToArray();
        List<(double px, double py, double pz, double vx, double vy, double vz)> hailstones = new();

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(' ').Where((n, i) => n != "" && i != 3).Select(n => double.Parse(n.TrimEnd(','))).ToList();

            hailstones.Add((line[0], line[1], line[2], line[3], line[4], line[5]));
        }

        long min = UnitTestsP1.ContainsKey(input) ? 7 : 200000000000000;
        long max = UnitTestsP1.ContainsKey(input) ? 27 : 400000000000000;

        long count = 0;
        foreach (var combo in hailstones.Combinations(2).Select(c => c.ToList()))
        {
            var ratio = combo[0].vx / combo[0].vy;
            var denominator = (combo[1].vx - combo[1].vy * ratio);

            if (denominator != 0)
            {
                var t = (combo[0].px - combo[0].py * ratio + combo[1].py * ratio - combo[1].px) / denominator;
                var k = (combo[1].px - combo[0].px + t * combo[1].vx) / combo[0].vx;

                if (t > 0 && k > 0)
                {
                    var pos = (X: combo[1].px + t * combo[1].vx, Y: combo[1].py + t * combo[1].vy, Z: combo[1].pz + t * combo[1].vz);

                    if (min <= pos.X && pos.X <= max && min <= pos.Y && pos.Y <= max)
                    {
                        count++;
                    }

                }
            }
        }


        return $"{count}";
    }

    public string SolvePart2(string input)
    {
        List<long[]> hailstones = input.Split(Environment.NewLine).Select(l => l.Split(' ').Where((n, i) => n != "" && i != 3).Select(n => long.Parse(n.TrimEnd(','))).ToArray()).ToList();

        var h1 = hailstones[0];
        var h2 = hailstones[1];

        const int range = 200;

        for (var i = -range; i < range; i++)
        {
            for (var j = -range; j < range; j++)
            {
                var det = (h1[3] - i) * (j - h2[4]) - (i - h2[3]) * (h1[4] - j);
                if (det == 0) continue;

                var t1 = 1d / det * ((j - h2[4]) * (h2[0] - h1[0]) + (h2[3] - i) * (h2[1] - h1[1]));
                var t2 = 1d / det * ((j - h1[4]) * (h2[0] - h1[0]) + (h1[3] - i) * (h2[1] - h1[1]));

                var x = (long)Math.Round(h1[0] + (h1[3] - i) * t1);
                var y = (long)Math.Round(h1[1] + (h1[4] - j) * t1);

                var k = (long)Math.Round((h2[2] + h2[5] * t2 - h1[2] - h1[5] * t1) / (t2 - t1));
                var z = (long)Math.Round(h1[2] + (h1[5] - k) * t1);

                var line = new long[] { x, y, z, i, j, k };

                if (hailstones.Where((l, x) => x != i && x != j).All(l => Intersect(line, l)))
                {
                    return $"{x + y + z}";
                }
            }
        }

        return "Not found within estimated range";
    }

    static bool Intersect(long[] l1, long[] l2)
        => Enumerable.Range(0, 3).Where(i => (l2[i + 3] - l1[i + 3]) != 0).Select(i => (double)(l1[i] - l2[i]) / (l2[i + 3] - l1[i + 3])).Distinct().Count() == 1;
}