namespace AdventOfCode2023;
public class Day11 : IDay
{
    public int Day => 11;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....", "374" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....", "82000210" }
    };

    public string SolvePart1(string input) => $"{Solve(input, 2)}";

    public string SolvePart2(string input) => $"{Solve(input, 1_000_000)}";

    private static long Solve(string input, int expansionFactor)
        => new string[][] { input.Split(Environment.NewLine) }
            .Select(lines => new
            {
                emptyRows = lines.Select((l, i) => (l, i)).Where(l => l.l.All(c => c != '#')).Select(l => l.i).ToList(),
                emptyCols = lines.Transpose().Select((l, i) => (l, i)).Where(l => l.l.All(c => c != '#')).Select(l => l.i).ToList(),
                galaxies = lines.Select((l, i) => (l, i)).SelectMany(l => l.l.Select((c, i) => (c, i)).Where(c => c.c == '#').Select(c => new Point(c.i, l.i))).ToList()
            })
            .Select(data =>
                data.galaxies.Select(g => new Point(data.emptyCols.TakeWhile(c => c < g.X).Count() * (expansionFactor - 1) + g.X, data.emptyRows.TakeWhile(r => r < g.Y).Count() * (expansionFactor - 1) + g.Y))
                .Combinations(2).Select(i => i.ToArray())
                .Sum(i => i[0].MDistanceTo(i[1])))
            .First();
}