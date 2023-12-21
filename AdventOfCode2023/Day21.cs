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
        { "500...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "167004" },
        { "1000...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "668697" }
        // { "5000...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........", "16733044" } // this one takes quite a lot of time to run
    };
    
    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public string SolvePart1(string input)
    {
        int steps = UnitTestsP1.ContainsKey(input) ? 6 : 64;

        (char[][] map, Point start) = ParseInput(input);
        return $"{Solve(map, start, steps)}";
    }

    public IEnumerable<Point> Explore(Grid<char> map, Point location, int steps, Dictionary<(Point, int), HashSet<Point>> memo)
    {
        if (steps == 0)
        {
            yield return location;
            yield break;
        }
        if (memo.TryGetValue((location, steps), out var result))
        {
            foreach (var point in result) yield return point;
            yield break;
        }

        HashSet<Point> visited = [];

        foreach (var loc in map.Adjacents(location).Where(p => map[p] != '#').SelectMany(adj => Explore(map, adj, steps - 1, memo)))
        {
            if (visited.Add(loc))
                yield return loc;
        }

        memo[(location, steps)] = visited;
    }
    public string SolvePart2(string input)
    {
        // test inputs
        if (input[0] != '.')
        {
            var temp = string.Concat(input.TakeWhile(x => x != '.'));
            var testSteps = long.Parse(temp);
            (char[][] testMap, Point testStart) = ParseInput(input[temp.Length..]);
            return $"{Solve(testMap, testStart, testSteps)}";
        }
        long steps = 26501365;

        (char[][] map, Point start) = ParseInput(input);

        // THIS SOLUTION COMPLETELY RELIES ON THE ASSUMPTION THAT THE ROW AND COLUMN UPON WHICH S LIES IN ARE FREE OF ANY ROCKS

        // the following points assume the form ax^2 + bx + c

        //     (65 is half the map width       131 = (steps - 65)          essentially how many 
        //      and the location of S)         / map width               functional copies we have
        // (0, Solve(input, 65)), (1, Solve(input, 65 + 131)), (2, Solve(input, 65 + 131 + 131))

        // x=0:           c     =>   ||  c == y0  ||
        // x=1:  a +  b + c       
        // x=2: 4a + 2b + c   

        //  (x=2) - 2 * (x=1)
        //       y2 = 4a + 2b +  c
        //      2y1 = 2a + 2b + 2c  -
        // y2 - 2y1 = 2a - c

        // ||  a = (y2 - 2y1 + c) / 2  ||
        // ||  b = y1 - c - a  ||


        long x0 = start.Y, x1 = x0 + map.Length, x2 = x1 + map.Length;
        long y0 = Solve(map, start, x0), y1 = Solve(map, start, x1), y2 = Solve(map, start, x2);

        long c = y0;
        long a = (y2 - 2 * y1 + c) / 2;
        long b = y1 - c - a;

        long xAns = (steps - x0) / map.Length;

        return $"{a * xAns * xAns + b * xAns + c}";
    }

    private static (char[][] map, Point start) ParseInput(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        Point start = map.SelectMany((r, i) => r.Select((c, j) => (c, new Point(i, j)))).First(t => t.c == 'S').Item2;

        return (map, start);
    }

    private static long Solve(char[][] map, Point start, long steps)
    {
        Point lb = start, ub = start, oldlb = start, oldub = start;

        long oddFixed = 0;
        long evenFixed = 0;

        HashSet<Point> oddContained = [];
        HashSet<Point> evenContained = [];
        HashSet<Point> edges = [start];

        for (int i = 0; i < steps; i++)
        {
            HashSet<Point> newEdges = [];

            Point newlb = lb;
            Point newub = ub;

            HashSet<Point> toRemove = [];

            foreach (var edge in edges)
            {
                foreach (var point in vectors.Select(v => edge + v))
                {
                    if (i % 2 == 0 ? oddContained.Contains(point) : evenContained.Contains(point))
                    {
                        toRemove.Add(point);
                    }
                    else
                    {
                        if (map[Utils.Mod(point.Y, map.Length)][Utils.Mod(point.X, map[0].Length)] != '#')
                        {
                            newEdges.Add(point);
                        }
                    }
                }
            }

            foreach (var edgeForRemoval in toRemove)
            {
                if (i % 2 == 0)
                {
                    oddContained.Remove(edgeForRemoval);
                    oddFixed++;
                }
                else
                {
                    evenContained.Remove(edgeForRemoval);
                    evenFixed++;
                }
            }

            edges = newEdges;
            oldlb = lb;
            oldub = ub;
            lb = newlb;
            ub = newub;

            if (i % 2 == 0)
            {
                foreach (var e in edges)
                    oddContained.Add(e);
            }
            else
            {
                foreach (var e in edges)
                    evenContained.Add(e);
            }
        }

        return steps % 2 == 0 ? evenFixed + evenContained.Count : oddFixed + oddContained.Count;
    }
}