using System.Diagnostics.Tracing;

namespace AdventOfCode2023;
public class Day21 : IDay
{
    public int Day => 21;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........",
            "16"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "...........\r\n.....###.#.\r\n.###.##..#.\r\n..#.#...#..\r\n....#.#....\r\n.##..S####.\r\n.##..#...#.\r\n.......##..\r\n.##.#.####.\r\n.##..##.##.\r\n...........",
            "16"
        },

    };

    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public string SolvePart1(string input)
    {
        long total = 0;

        var map = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());
        Point start = (-1, -1);
        for (int i = 0; i < map.Width && start.X < 0; i++)
        {
            for (int j = 0; j < map.Height; j++)
            {
                if (map[i, j] == 'S')
                {
                    start = (i, j);
                    break;
                }
            }
        }

        int steps = UnitTestsP1.ContainsKey(input) ? 6 : 64;

        return $"{Explore(map, start, steps, []).Count()}";
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
    long mod(long x, int m) => (x % m + m) % m;
    public string SolvePart2(string input)
    {
        // the following points assume the form ax^2 + bx + c
        // (0, Solve(input, 65)), (1, Solve(input, 65 + 131)), (2, Solve(input, 65 + 131 + 131))

        // x=0:           c   => c == y0
        // x=1:  a +  b + c       
        // x=2: 4a + 2b + c   

        //  (x=2) - 2 * (x=1)
        //       y2 = 4a + 2b +  c
        //      2y1 = 2a + 2b + 2c  -
        // y2 - 2y1 = 2a - c
        
        // a = (y2 - 2y1 + c) / 2

        
        // b = y1 - c - a


        int x0 = 65, x1 = 65 + 131, x2 = 65 + 131 + 131;
        long y0 = Solve(input, x0), y1 = Solve(input, x1), y2 = Solve(input, x2);

        long c = y0;
        long a = (y2 - 2 * y1 + c) / 2;
        long b = y1 - c - a;

        long xAns = 202300;

        return $"{a * xAns * xAns + b * xAns + c}";

        //int steps = UnitTestsP2.ContainsKey(input) ? 0 : 65;
    }

    long Solve(string input, int steps)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        Point start = (-1, -1);
        for (int i = 0; i < map.Length && start.Y < 0; i++)
        {
            for (int j = 0; j < map[0].Length; j++)
            {
                if (map[i][j] == 'S')
                {
                    start = (i, j);
                    break;
                }
            }
        }

        Point lb = start, ub = start, oldlb = start, oldub = start;

        long oddFixed = 0;
        long evenFixed = 0;

        HashSet<Point> oddContained = [];
        HashSet<Point> evenContained = [];
        HashSet<Point> oldEdges = [];
        HashSet<Point> edges = [start];
        //Console.SetCursorPosition((int)start.X, (int)start.Y);
        //Console.Write('S');
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
                    if ((i % 2 == 0 ? oddContained.Contains(point) : evenContained.Contains(point)))
                    {
                        toRemove.Add(point);
                    }
                    else
                    {
                        if (map[mod(point.Y, map.Length)][mod(point.X, map[0].Length)] != '#')
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

            oldEdges = edges;
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

            //Console.WriteLine($"Step {i} complete");
        }

        return steps % 2 == 0 ? evenFixed + evenContained.Count : oddFixed + oddContained.Count;
    }
}