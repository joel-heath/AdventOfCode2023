namespace AdventOfCode2023;
public class Day17 : IDay
{
    public int Day => 17;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "2413432311323\r\n3215453535623\r\n3255245654254\r\n3446585845452\r\n4546657867536\r\n1438598798454\r\n4457876987766\r\n3637877979653\r\n4654967986887\r\n4564679986453\r\n1224686865563\r\n2546548887735\r\n4322674655533", "102" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "2413432311323\r\n3215453535623\r\n3255245654254\r\n3446585845452\r\n4546657867536\r\n1438598798454\r\n4457876987766\r\n3637877979653\r\n4654967986887\r\n4564679986453\r\n1224686865563\r\n2546548887735\r\n4322674655533", "94" },
        { "111111111111\r\n999999999991\r\n999999999991\r\n999999999991\r\n999999999991", "71" }
    };

    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public static long FindShortestPath(Grid<int> map, Point start, Point goal)
    {
        var min = long.MaxValue;

        //Dictionary<(Point, int, int), long> visited = new();
        HashSet<(Point, int, int)> visited = new();
        PriorityQueue<(Point loc, long dist, int dir, int str), long> queue = new([((start, 0, 1, 1), 0)]);

        while (queue.TryDequeue(out var result, out _))
        {
            if (result.dist >= min) continue;
            if (result.loc == goal)
            {
                min = Math.Min(min, result.dist);
                //Console.WriteLine(min);
                continue;
            }
            if (visited.Contains((result.loc, result.dir, result.str))) continue;
            visited.Add((result.loc, result.dir, result.str));

            //left
            {
                var newDir = (result.dir + 3) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], newDir, 1), result.dist);
                }
            }

            // right
            {
                var newDir = (result.dir + 1) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], newDir, 1), result.dist);
                }
            }

            // straight
            if (result.str < 3)
            {
                var newPoint = result.loc + vectors[result.dir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], result.dir, result.str + 1), result.dist);
                }
            }
        }

        return min;
    }

    public static long FindShortestPath2(Grid<int> map, Point start, Point goal)
    {
        var min = long.MaxValue;

        HashSet<(Point, int, int)> visited = new();
        PriorityQueue<(Point loc, long dist, int dir, int str), long> queue = new([((start, 0, 1, 1), 0)]);

        while (queue.TryDequeue(out var result, out _))
        {
            if (result.dist + result.loc.MDistanceTo(goal) >= min) continue;
            if (result.loc == goal)
            {
                if (result.str >= 4) min = Math.Min(min, result.dist);
                continue;
            }
            if (visited.Contains((result.loc, result.dir, result.str))) continue;
            visited.Add((result.loc, result.dir, result.str));

            //left
            if (result.str >= 4)
            {
                var newDir = (result.dir + 3) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], newDir, 1), result.dist);
                }

            // right
                newDir = (result.dir + 1) % 4;
                newPoint = result.loc + vectors[newDir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], newDir, 1), result.dist);
                }
            }

            // straight
            if (result.str < 10)
            {
                var newPoint = result.loc + vectors[result.dir];
                if (map.Contains(newPoint))
                {
                    queue.Enqueue((newPoint, result.dist + map[newPoint], result.dir, result.str + 1), result.dist);
                }
            }
        }

        return min;
    }

    public string SolvePart1(string input)
    {
        var map = new Grid<int>(input.Split(Environment.NewLine).Select(l => l.Select(i => int.Parse($"{i}")).ToArray()).ToArray());
        var goal = (map.Width - 1, map.Height - 1);

        return $"{FindShortestPath(map, (0, 0), goal)}";
    }

    public string SolvePart2(string input)
    {
        var map = new Grid<int>(input.Split(Environment.NewLine).Select(l => l.Select(i => int.Parse($"{i}")).ToArray()).ToArray());
        var goal = (map.Width - 1, map.Height - 1);

        return $"{FindShortestPath2(map, (0, 0), goal)}";
    }
}