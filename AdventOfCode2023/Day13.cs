namespace AdventOfCode2023;
public class Day13 : IDay
{
    public int Day => 13;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#", "405" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#", "400" }
    };

    public string SolvePart1(string input) => $"{Solve(input, smudgeRules: false)}";
    
    public string SolvePart2(string input) => $"{Solve(input, smudgeRules: true)}";

    private static long Solve(string input, bool smudgeRules)
        => input.Split(Environment.NewLine + Environment.NewLine).Sum(pattern =>
                new char[][][] { pattern.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray() }
                .Select(lines => (lines, lines.Take(lines.Length - 1).Select((l, i) => (100 * (i + 1), IsLineOfReflection(lines, i, smudgeRules))).FirstOrDefault(i => i.Item2)))
                .Select(l => l.Item2.Item1 > 0 ? l.Item2.Item1
                    : new char[][][] { l.lines.Transpose().Select(l => l.ToArray()).ToArray() }
                        .Select(lines => lines.Take(lines.Length - 1).Select((l, i) => (i + 1, IsLineOfReflection(lines, i, smudgeRules))).First(i => i.Item2)).First().Item1).First());

    private static bool IsLineOfReflection(IEnumerable<IEnumerable<char>> input, int index, bool smudgeRules, bool haveFoundSmudge = false)
        => input.Take(index + 1).Reverse().Zip(input.Skip(index + 1))
            .All(t => t.First.Zip(t.Second).All(c => c.First == c.Second || (smudgeRules && (haveFoundSmudge = !haveFoundSmudge)))) && (haveFoundSmudge || !smudgeRules);
}