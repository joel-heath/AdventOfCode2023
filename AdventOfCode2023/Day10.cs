namespace AdventOfCode2023;
public class Day10 : IDay
{
    public int Day => 10;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { ".....\r\n.S-7.\r\n.|.|.\r\n.L-J.\r\n.....", "4" },
        { "-L|F7\r\n7S-7|\r\nL|7||\r\n-L-J|\r\nL|-JF", "4" },
        { "..F7.\r\n.FJ|.\r\nSJ.L7\r\n|F--J\r\nLJ...", "8" },
        { "7-F7-\r\n.FJ|7\r\nSJLL7\r\n|F--J\r\nLJ.LJ", "8" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "...........\r\n.S-------7.\r\n.|F-----7|.\r\n.||.....||.\r\n.||.....||.\r\n.|L-7.F-J|.\r\n.|..|.|..|.\r\n.L--J.L--J.\r\n...........", "4" },
        { ".F----7F7F7F7F-7....\r\n.|F--7||||||||FJ....\r\n.||.FJ||||||||L7....\r\nFJL7L7LJLJ||LJ.L-7..\r\nL--J.L7...LJS7F-7L7.\r\n....F-J..F7FJ|L7L7L7\r\n....L7.F7||L7|.L7L7|\r\n.....|FJLJ|FJ|F7|.LJ\r\n....FJL-7.||.||||...\r\n....L---J.LJ.LJLJ...", "8" },
        { "FF7FSF7F7F7F7F7F---7\r\nL|LJ||||||||||||F--J\r\nFL-7LJLJ||||||LJL-77\r\nF--JF--7||LJLJ7F7FJ-\r\nL---JF-JLJ.||-FJLJJ7\r\n|F|F-JF---7F7-L7L|7|\r\n|FFJF7L7F-JF7|JL---7\r\n7-L-JL7||F7|L7F-7F7|\r\nL.L7LFJ|||||FJL7||LJ\r\nL7JLJL-JLJLJL--JLJ.L", "10" }
    };

    private static readonly IEnumerable<Point> vectors = new Point[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
    private static readonly char[][] directions = [['|', 'F', '7', 'S'], ['-', 'J', '7', 'S'], ['|', 'J', 'L', 'S'], ['-', 'L', 'F', 'S']];

    public string SolvePart1(string input)
    => $"{new string[][] { input.Split(Environment.NewLine) }
        .Select(i => i.Select((l, i) => (l.Select((c, i) => (c, i)).ToArray(), i)).ToArray())
        .Select(i => (i.Select(l => l.Item1.Select(c => c.c).ToArray()).ToArray(),
            i.Select(l => (l.Item1.FirstOrDefault(c => c.c == 'S', (default, -1)).i, l.i))
                .First(l => l.Item1 != -1)))
        .Select(t => BFS(t.Item2, new Grid<char>(t.Item1)).Item2)
        .First()}";

    public string SolvePart2(string input)
    => $"{new string[][] { input.Split(Environment.NewLine) }.SelectMany(rawLines =>
        new IEnumerable<string>[] { rawLines.Select(l => string.Concat(new List<char> { '.' }.Concat(l).Append('.'))).Prepend(new string('.', rawLines[0].Length + 2)).Append(new string('.', rawLines[0].Length + 2)) }
            .Select(i => i.Select((l, i) => (l.Select((c, i) => (c, i)).ToArray(), i)).ToArray())
            .Select(i => (new Grid<char>(i.Select(l => l.Item1.Select(c => c.c).ToArray()).ToArray()),
                i.Select(l => (l.Item1.FirstOrDefault(c => c.c == 'S', (default, -1)).i, l.i))
                    .First(l => l.Item1 != -1)))
            .Select(t => ((t.Item1[t.Item2] = new bool[][] { vectors.Take(3).Select((v, i) => (i, t.Item1[t.Item2 + v])).Select(n => directions[n.i].Contains(n.Item2)).ToArray() }
                .Select(a => a[0] ? a[2] ? '|' : a[1] ? 'L' : 'J' : a[2] ? a[1] ? 'F' : '7' : '-').First())
                    == default ? t.Item1 : t.Item1, t.Item2)))
            .Select(i => (i.Item1, BFS(i.Item2, i.Item1).Item1))
            .Select<(Grid<char> grid, HashSet<Point> loop), int>(inp =>
                Enumerable.Range(0, inp.grid.Height).Sum(y => 
                    Enumerable.Range(0, inp.grid.Width)
                        .Sum(x => 
                        (!inp.loop.Contains((x, y)) &&
                            inp.grid.LineOut((x, y), 0, false).Where(p => inp.loop.Contains(p) && inp.grid[p] != '|')
                                .Aggregate((0, (char)0), (acc, point) =>
                                    (acc.Item2 != (char)0)
                                        ? (acc.Item1 + ((
                                            (acc.Item2 == 'F' && inp.grid[point] == 'L' || acc.Item2 == 'L' && inp.grid[point] == 'F'
                                            || acc.Item2 == '7' && inp.grid[point] == 'J' || acc.Item2 == 'J' && inp.grid[point] == '7')) ? 1 : 0),
                                                (char)0)
                                        : (acc.Item1 + 1, (inp.grid[point] != '-' && inp.grid[point] != '|') ? inp.grid[point] : acc.Item2)
                                ).Item1 % 2 == 1) ? 1 : 0))).First()}";

    private static (HashSet<Point>, int) BFS(Point start, Grid<char> map)
    {
        HashSet<Point> visited = [];
        Queue<(Point, int)> toVisit = new([(start, 0)]);

        int max = 0;
        while (toVisit.TryDequeue(out var node))
        {
            visited.Add(node.Item1);
            max = Math.Max(max, node.Item2);
            foreach (var neighbor in vectors.Select(v => (node.Item1, node.Item1 + v, v)).Where(n => map.Contains(n.Item2) &&
                new (char current, char destination)[] { (map[n.Item1], map[n.Item2]) }
                    .Select(t => n.v switch
                    {
                        (0, -1) => directions[2].Contains(t.current) && directions[0].Contains(t.destination),
                        (0, 1) => directions[0].Contains(t.current) && directions[2].Contains(t.destination),
                        (1, 0) => directions[3].Contains(t.current) && directions[1].Contains(t.destination),
                        (-1, 0) => directions[1].Contains(t.current) && directions[3].Contains(t.destination),
                        _ => false
                    }).First()).Select(n => n.Item2)
                .Where(n => !visited.Contains(n) && !toVisit.Any(t => t.Item1 == n)))
            { toVisit.Enqueue((neighbor, node.Item2 + 1)); }
        }
        return (visited, max);
    }
}