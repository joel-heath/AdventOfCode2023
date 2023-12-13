using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public partial class Day08 : IDay
{
    public int Day => 8;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "RL\r\n\r\nAAA = (BBB, CCC)\r\nBBB = (DDD, EEE)\r\nCCC = (ZZZ, GGG)\r\nDDD = (DDD, DDD)\r\nEEE = (EEE, EEE)\r\nGGG = (GGG, GGG)\r\nZZZ = (ZZZ, ZZZ)", "2" },
        { "LLR\r\n\r\nAAA = (BBB, BBB)\r\nBBB = (AAA, ZZZ)\r\nZZZ = (ZZZ, ZZZ)", "6" }

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)", "6" }
    };

    public string SolvePart1(string input)
        => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }.Select(lines =>
            NodeFinder("AAA", Node().Matches(lines[1]).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0].Value))
                    .Chunk(3)
                    .ToDictionary(i => i[0], i => (i[1], i[2])),
                    lines[0], true))
            .First()}";

    public string SolvePart2(string input)
        => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }
            .Select(lines => (Node().Matches(lines[1]).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0].Value))
                .Chunk(3)
                .ToDictionary(i => i[0], i => (i[1], i[2])),
                lines[0]))
            .Select(d => d.Item1.Keys.Where(k => k[2] == 'A')
                .Select(c => NodeFinder(c, d.Item1, d.Item2, false))
                .Aggregate(Utils.LCM))
            .First()}";

    private static long NodeFinder(string start, Dictionary<string, (string, string)> graph, string instructions, bool tripleZ)
    {
        long j = 0;
        for (; tripleZ && start != "ZZZ" || !tripleZ && start[2] != 'Z'; j++)
            start = new (string left, string right)[] { graph[start] }.Select(t => instructions[(int)(j % instructions.Length)] == 'L' ? t.left : t.right).First();
        return j;
    }

    [GeneratedRegex(@"\w{3}")]
    private static partial Regex Node();
}