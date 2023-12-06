namespace AdventOfCode2023;
public class Day06 : IDay
{
    public int Day => 6;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Time:      7  15   30\r\nDistance:  9  40  200", "288" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Time:      7  15   30\r\nDistance:  9  40  200", "71503" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Select(l => l.Split(' ').Skip(1).Where(c => c != string.Empty).Select(int.Parse))
            .SelectMany(inner => inner.Select((item, index) => new { item, index }))
            .GroupBy(i => i.index, i => i.item)
            .Select(g => g.ToArray()).ToArray()
            .Select(l => (l[0] / 2.0, Math.Sqrt((double)l[0] * l[0] / 4.0 - l[1])))
            .Select(l => (int)Math.Ceiling(l.Item1 + l.Item2) - (int)Math.Floor(l.Item1 - l.Item2) - 1)
            .Aggregate((a, i) => a * i)}";

    public string SolvePart2(string input)
        => $"{new long[][] { input.Split(Environment.NewLine).Select(l => l.Split(' ').Skip(1).Where(c => c != string.Empty)
            .Aggregate((a, c) => a + c)).Select(long.Parse).ToArray() }
            .Select(l => (l[0] / 2.0, Math.Sqrt((double)l[0] * l[0] / 4.0 - l[1])))
            .Select(l => (int)Math.Ceiling(l.Item1 + l.Item2) - (int)Math.Floor(l.Item1 - l.Item2) - 1)
            .First()}";
}