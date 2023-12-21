namespace AdventOfCode2023;
public class Day21 : IDay
{
    public int Day => 21;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "16" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "6...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "16" },
        { "10...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "50" },
        { "50...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "1594" },
        { "100...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "6536" },
        //{ "500...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "167004" },
        //{ "1000...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "668697" }
        // { "5000...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "16733044" } // this one takes quite a lot of time to run
    };
    
    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public string SolvePart1(string input)
        => $"{new char[][][] { input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray() }
            .Select(map => (map, start:map.SelectMany((r, i) => r.Select((c, j) => (c, new Point(i, j)))).First(t => t.c == 'S').Item2))
            .Sum(d => Solve(d.map, d.start, UnitTestsP1.ContainsKey(input) ? 6 : 64))}";

    public string SolvePart2(string input)
        => $"{((input[0] != '.') // test inputs
            ? new string[] { string.Concat(input.TakeWhile(x => x != '.')) }
                .Select(num => (num, d:ParseInput(input[num.Length..])))
                .Sum(d => Solve(d.d.map, d.d.start, long.Parse(d.num)))
            : new (char[][] map, Point start)[] { ParseInput(input) }.Sum(z => 
                new Point[][] { new long[] { z.start.Y, z.start.Y + z.map.Length, z.start.Y + 2 * z.map.Length }
                    .Select(x => new Point(x, Solve(z.map, z.start, x))).ToArray() }
                    .Select(p => (x: (26501365 - p[0].X) / z.map.Length, p))
                    .Sum(d => ((d.p[2].Y - 2 * d.p[1].Y + d.p[0].Y) / 2 * d.x + (2 * d.p[1].Y - (3 * d.p[0].Y - d.p[2].Y) / 2)) * d.x + d.p[0].Y)))}";

    private static (char[][] map, Point start) ParseInput(string input)
        => new char[][][] { input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray() }
            .Select(map => (map, map.SelectMany((r, i) => r.Select((c, j) => (c, new Point(i, j)))).First(t => t.c == 'S').Item2))
            .First();

    private static long Solve(char[][] map, Point start, long steps)
        => new (HashSet<Point> oddContained, HashSet<Point> evenContained, HashSet<Point> edges, long oddFixed, long evenFixed, Point lb, Point ub, Point oldlb, Point newlb)[]
            { ([], [], [start], 0, 0, start, start, start, start) }
            .Sum(z => Utils.Range(0, steps).Select(i =>
                    z = new (HashSet<Point> newEdges, HashSet<Point> toRemove, Point newlb, Point newub)[] { ([], [], z.lb, z.ub) }
                        .Select(d => (z.edges.All(edge =>
                                vectors.Select(v => edge + v).Where(point => map[Utils.Mod(point.Y, map.Length)][Utils.Mod(point.X, map[0].Length)] != '#')
                                .All(point => ((i % 2 == 0 ? z.oddContained.Contains(point) : z.evenContained.Contains(point)) ? d.toRemove : d.newEdges).Add(point) || true))
                            && d.toRemove.Select(edgeForRemoval => i % 2 == 0 ? (z.oddContained.Remove(edgeForRemoval) ? z.oddFixed++ : z.oddFixed++) : (z.evenContained.Remove(edgeForRemoval) ? z.evenFixed++ : z.evenFixed++)).All(i => i > -1)
                            && (i % 2 == 0) ? d.newEdges.All(e => z.oddContained.Add(e) || true) : d.newEdges.All(e => z.evenContained.Add(e) || true)) ? d : d)
                            .Select(d => (z.oddContained, z.evenContained, d.newEdges, z.oddFixed, z.evenFixed, d.newlb, d.newub, z.lb, z.ub))
                        .First()).All(x => x != default || true)
                ? (steps % 2 == 0 ? z.evenFixed + z.evenContained.Count : z.oddFixed + z.oddContained.Count) : 0);
}