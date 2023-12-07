namespace AdventOfCode2023;
public class Day07 : IDay
{
    public int Day => 7;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483", "6440" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483", "5905" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Select(l => l.Split(" ")).Select(l => (l[0].ToCharArray(), long.Parse(l[1])))
                .OrderBy(h => new long[][] { [.. h.Item1.GroupBy(c => c).Select(g => (long)g.Count()).OrderDescending()] }
                .Sum(g => g[0] * 1100000
                    + (g[0] == 2 && g[1] == 2 ? 540000 : 0)
                    + (g[0] == 3 && g[1] == 2 ? 540000 : 0))
                    + h.Item1.Select((c, i) => (long)Math.Pow(14, 4 - i) * (c switch { 'A' => 14, 'K' => 13, 'Q' => 12, 'J' => 11, 'T' => 10, _ => int.Parse($"{c}") })).Sum())
                .Select((c, i) => (i + 1) * c.Item2).Sum()}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine).Select(l => l.Split(" ")).Select(l => (l[0].ToCharArray(), long.Parse(l[1])))
                .OrderBy(h => new long[][] { [.. h.Item1.Where(i => i != 'J').GroupBy(c => c).Select(g => (long)g.Count()).OrderDescending()] }
                    .Select(g => g.Length == 0 ? new long[] { 5 } : [g[0] + h.Item1.Where(c => c == 'J').Count(), .. g[1..]])
                    .Sum(g => g[0] * 1100000
                        + (g[0] == 2 && g[1] == 2 ? 540000 : 0)
                        + (g[0] == 3 && g[1] == 2 ? 540000 : 0))
                        + h.Item1.Select((c, i) => (long)Math.Pow(14, 4 - i) * (c switch { 'A' => 14, 'K' => 13, 'Q' => 12, 'T' => 10, 'J' => 1, _ => int.Parse($"{c}") })).Sum())
                .Select((c, i) => (i + 1) * c.Item2).Sum()}";
}