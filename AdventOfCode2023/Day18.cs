namespace AdventOfCode2023;
public class Day18 : IDay
{
    public int Day => 18;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", "62" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "R 6 (#70c710)\r\nD 5 (#0dc571)\r\nL 2 (#5713f0)\r\nD 2 (#d2c081)\r\nR 2 (#59c680)\r\nD 2 (#411b91)\r\nL 5 (#8ceee2)\r\nU 2 (#caa173)\r\nL 1 (#1b58a2)\r\nU 2 (#caa171)\r\nR 2 (#7807d2)\r\nU 3 (#a77fa3)\r\nL 2 (#015232)\r\nU 2 (#7a21e3)", "952408144115" }
    };

    private static readonly Point[] vectors = [(1, 0), (0, 1), (-1, 0), (0, -1)];

    public string SolvePart1(string input)
        => $"{new List<Point>[] { [(0, 0)] }.Select(points =>
            input.Split(Environment.NewLine).Aggregate(0L, (acc, l) =>
                new string[][] { l.Split(' ') }.Select(d => (vectors[d[0][0] switch { 'R' => 0, 'D' => 1, 'L' => 2, _ => 3 }], int.Parse(d[1])))
                    .Select(d => new Action(() => points.Add(points[^1] + d.Item2 * d.Item1)).InvokeTruthfully() ? acc + d.Item2 : 0).First()) / 2
                + points.Take(points.Count - 1).Select((p, i) => (p.X * points[i + 1].Y) - (points[i + 1].X * p.Y)).Sum() / 2 + 1).First()}";

    public string SolvePart2(string input)
        => $"{new List<Point>[] { [(0, 0)] }.Select(points =>
            input.Split(Environment.NewLine).Aggregate(0L, (perimeter, line) =>
                new string[] { line.Split('#')[1][..^1] }.Select(l =>
                    (vectors[int.Parse($"{l[^1]}")], int.Parse(l[..^1], System.Globalization.NumberStyles.HexNumber)))
                    .Select(d => new Action(() => points.Add(points[^1] + d.Item2 * d.Item1)).InvokeTruthfully() ? perimeter + d.Item2 : 0).First()) / 2
                + points.Take(points.Count - 1).Select((p, i) => (p.X * points[i + 1].Y) - (points[i + 1].X * p.Y)).Sum() / 2 + 1).First()}";
}