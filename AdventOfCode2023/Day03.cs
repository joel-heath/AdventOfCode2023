using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public partial class Day03 : IDay
{
    public int Day => 3;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "4361" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "467835" }

    };

    public string SolvePart1(string input)
        => $"{new string[][] { input.Split(Environment.NewLine) }
            .Select(lines => lines.Select((l, i) =>
            lines.Select((l, i) =>
            DigitsRegex().Matches(l).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0]))
                .Where(n =>
                    Enumerable.Range(0, n.Length).Select(j => (i - 1, n.Index + j)).Concat(Enumerable.Range(0, n.Length).Select(j => (i + 1, n.Index + j)))
                    .Concat(Enumerable.Range(-1, 3).Select(j => (i + j, n.Index - 1))).Concat(Enumerable.Range(-1, 3).Select(j => (i + j, n.Index + n.Length)))
                    .Where(c => c.Item1 >= 0 && c.Item1 < lines.Length && c.Item2 >= 0 && c.Item2 < lines[c.Item1].Length)
                    .Select(c => lines[c.Item1][c.Item2])
                    .Any(i => i != '.' && (i < '0' || i > '9')))
                .Sum(n => int.Parse(n.Value)))
            .Sum())).First()}";

    public string SolvePart2(string input)
        => $"{new string[][] { input.Split(Environment.NewLine) }
            .Select(lines => lines.Select((l, i) => GearsRegex().Matches(l).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0]))
                .Select(g => new (int, int)[] { (i - 1, g.Index), (i + 1, g.Index) }
                    .Concat(Enumerable.Range(-1, 3).Select(j => (i + j, g.Index - 1))).Concat(Enumerable.Range(-1, 3).Select(j => (i + j, g.Index + 1)))
                    .Where(c => c.Item1 >= 0 && c.Item1 < lines.Length && c.Item2 >= 0 && c.Item2 < lines[c.Item1].Length)
                    .Where(i => lines[i.Item1][i.Item2] >= '0' && lines[i.Item1][i.Item2] <= '9')
                    .Select(n => (n.Item1, n.Item2,
                        string.Concat(lines[n.Item1][..n.Item2].Reverse().TakeWhile(c => '0' <= c && c <= '9').Reverse()), string.Concat(lines[n.Item1][(n.Item2 + 1)..].TakeWhile(c => '0' <= c && c <= '9'))))
                    .Select(t => (t.Item1, t.Item2, t.Item3, t.Item4, t.Item2 - t.Item3.Length, t.Item2 + t.Item4.Length))
                    .DistinctBy(t => (t.Item1, t.Item5, t.Item6))
                    .Select(t => t.Item3 + lines[t.Item1][t.Item2] + t.Item4)
                    .Select(int.Parse)
                    .ToArray())
                .Where(g => g.Length == 2)
                .Sum(g => g.Aggregate((acc, v) => acc * v)))
            .Sum()).First()}";

    [GeneratedRegex(@"\d+")]
    private static partial Regex DigitsRegex();
    [GeneratedRegex(@"\*")]
    private static partial Regex GearsRegex();
}
