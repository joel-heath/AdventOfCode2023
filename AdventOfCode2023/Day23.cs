namespace AdventOfCode2023;
public class Day23 : IDay
{
    public int Day => 23;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "#.#####################\r\n#.......#########...###\r\n#######.#########.#.###\r\n###.....#.>.>.###.#.###\r\n###v#####.#v#.###.#.###\r\n###.>...#.#.#.....#...#\r\n###v###.#.#.#########.#\r\n###...#.#.#.......#...#\r\n#####.#.#.#######.#.###\r\n#.....#.#.#.......#...#\r\n#.#####.#.#.#########v#\r\n#.#...#...#...###...>.#\r\n#.#.#v#######v###.###v#\r\n#...#.>.#...>.>.#.###.#\r\n#####v#.#.###v#.#.###.#\r\n#.....#...#...#.#.#...#\r\n#.#########.###.#.#.###\r\n#...###...#...#...#.###\r\n###.###.#.###v#####v###\r\n#...#...#.#.>.>.#.>.###\r\n#.###.###.#.###.#.#v###\r\n#.....###...###...#...#\r\n#####################.#", "94" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "#.#####################\r\n#.......#########...###\r\n#######.#########.#.###\r\n###.....#.>.>.###.#.###\r\n###v#####.#v#.###.#.###\r\n###.>...#.#.#.....#...#\r\n###v###.#.#.#########.#\r\n###...#.#.#.......#...#\r\n#####.#.#.#######.#.###\r\n#.....#.#.#.......#...#\r\n#.#####.#.#.#########v#\r\n#.#...#...#...###...>.#\r\n#.#.#v#######v###.###v#\r\n#...#.>.#...>.>.#.###.#\r\n#####v#.#.###v#.#.###.#\r\n#.....#...#...#.#.#...#\r\n#.#########.###.#.#.###\r\n#...###...#...#...#.###\r\n###.###.#.###v#####v###\r\n#...#...#.#.>.>.#.>.###\r\n#.###.###.#.###.#.#v###\r\n#.....###...###...#...#\r\n#####################.#", "154" }
    };

    public string SolvePart1(string input)
    {
        long total = 0;

        var map = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());
        var start = (1, 0);
        var end = (map.Width - 2, map.Height - 1);

        return $"{Explore(map, start, end)}";
    }

    static long Explore(Grid<char> map, Point start, Point end)
    {
        long max = 0;

        var stack = new Stack<(Point pos, HashSet<Point> visited, long dist)>([(start, [], 0)]);
        while (stack.TryPop(out var temp))
        {
            var (pos, visited, dist) = temp;
            var type = map[pos];

            if (pos == end)
            {
                max = Math.Max(max, dist);
                continue;
            }

            visited.Add(pos);

            var destinations = type switch
            {
                '.' => map.Adjacents(pos),
                '>' => Enumerable.Empty<Point>().Append(pos + (1, 0)),
                'v' => Enumerable.Empty<Point>().Append(pos + (0, 1)),
                '<' => Enumerable.Empty<Point>().Append(pos + (-1, 0)),
                '^' => Enumerable.Empty<Point>().Append(pos + (0, -1))
            };


            foreach (var des in destinations.Where(des => map[des] != '#' && !visited.Contains(des)).OrderBy(d => d.MDistanceTo((0, 0))))
            {
                stack.Push((des, visited.ToHashSet(), dist + 1));
            }
        }

        return max;
    }

    public string SolvePart2(string input)
    {
        long total = 0;
        string[] lines = input.Split(Environment.NewLine);
        Grid<char> map = new(lines.Length, lines[0].Length);

        var start = (1, 0);
        var end = (map.Width - 2, map.Height - 1);
        HashSet<Point> poi = [start,end];

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                map[x, y] = line[x];
                if (line[x] != '#')
                {
                    if (map.Adjacents((x, y)).Count(p => map[p] != '#') > 2)
                        poi.Add((x, y));
                }
            }
        }

        var edges = new Dictionary<Point, HashSet<(Point, long)>>();

        foreach (var point in poi)
        {
            edges[point] = Dijkstras(map, poi, point);
        }

        long max = 0;

        HashSet<Point> seenInitial = [start];
        Queue<(Point, HashSet<Point>, long)> todo = new([(start, seenInitial, 0)]);

        while (todo.TryDequeue(out var p))
        {
            var (pos, seen, cost) = p;

            if (pos == end)
            {
                max = Math.Max(max, cost);
                continue;
            }

            foreach (var (next, extra) in edges[pos])
            {
                if (!seen.Contains(next))
                {
                    var copy = seen.ToHashSet();
                    copy.Add(next);
                    todo.Enqueue((next, copy, cost + extra));
                }
            }
        }

        return $"{max}";
    }

    static HashSet<(Point, long)> Dijkstras(Grid<char> grid, HashSet<Point> poi, Point start)
    {
        Queue<(Point, long)> queue = [];
        HashSet<Point> visited = [];
        HashSet<(Point, long)> distances = [];

        queue.Enqueue((start, 0));
        visited.Add(start);

        while (queue.TryDequeue(out var p))
        {
            var (pos, cost) = p;

            if (pos != start && poi.Contains(pos))
            {
                distances.Add((pos, cost));
                continue;
            }

            foreach (var next in grid.Adjacents(pos).Where(next => grid.Contains(next) && grid[next] != '#' && visited.Add(next)))
            {
                queue.Enqueue((next, cost + 1));
            }
        }

        return distances;
    }
}