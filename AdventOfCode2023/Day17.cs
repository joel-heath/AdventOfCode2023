using System.Drawing;
using System.Linq;

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
    
    /*
    public long FindShortestPath(Grid<char> map, Point start, int dir, int straight)
    {
        Dictionary<Point, int> visited = [];
        Dictionary<Point, int> workingValues = new() { { start, 0 } };

        while (workingValues.Count > 0)
        {
            var (point, distance) = workingValues.MinBy(kvp => kvp.Value);

            visited[point] = distance;

            foreach (var newPoint in map.Adjacents(point).Where(p => !visited.ContainsKey(point)))
            {
                var newDistance = distance + map[newPoint];
                if (workingValues.TryGetValue(newPoint, out int oldDistance))
                {
                    if (newDistance < oldDistance)
                        workingValues[newPoint] = newDistance;
                }
                else workingValues[newPoint] = newDistance;
            }
        
        }

    }*/

    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    private static Dictionary<Point, long> memo = new();

    /*
    public static long FindShortestPath(Grid<int> map, Point start, Point goal, int dir, int straight)
    {
        if (start == goal) return 0;
        if (memo.TryGetValue(start, out var distance)) return distance;

        var min = long.MaxValue;

        //left
        Dictionary<Point, (long dist, int dir, int str)> options = new();
        {
            var newDir = (dir + 3) % 4;
            var newPoint = start + vectors[newDir];
            if (map.IsInGrid(newPoint))
            {
                //min = Math.Min(min, map[newPoint] + FindShortestPath(map, newPoint, newDir, 1));
                options[newPoint] = (map[newPoint], newDir, 1);
            }
        }
        // right
        {
            var newDir = (dir + 1) % 4;
            var newPoint = start + vectors[newDir];
            if (map.IsInGrid(newPoint))
            {
                //min = Math.Min(min, map[newPoint] + FindShortestPath(map, newPoint, newDir, 1));
                options[newPoint] = (map[newPoint], newDir, 1);
            }
        }
        // straight
        {
            var newPoint = start + vectors[dir];
            if (map.IsInGrid(newPoint) && straight < 3)
            {
                //min = Math.Min(min, map[newPoint] + FindShortestPath(map, newPoint, dir, straight + 1));
                options[newPoint] = (map[newPoint], dir, straight + 1);
            }
        }

        foreach (var pair in options.OrderBy(o => (goal - o.Key).X + (goal - o.Key).Y))
        {
            min = Math.Min(min, pair.Value.dist + FindShortestPath(map, pair.Key, goal, pair.Value.dir, pair.Value.str));
        }

        memo[start] = min;
        return min;
    }*/

    public static long FindShortestPath(Grid<int> map, Point start, Point goal)
    {
        var min = long.MaxValue;

        Dictionary<(Point, int, int), long> visited = new();
        Stack<(Point loc, long dist, int dir, int str)> stack = new([(start, 0, 1, 1), (start, 0, 2, 1)]);

        while (stack.TryPop(out var result))
        {
            if (result.dist >= min) continue;
            if (result.loc == goal)
            {
                min = Math.Min(min, result.dist);
                //Console.WriteLine(min);
                continue;
            }
            if (visited.TryGetValue((result.loc, result.dir, result.str), out var distance) && distance <= result.dist) continue;
            visited[(result.loc, result.dir, result.str)] = result.dist;

            Dictionary<Point, (long dist, int dir, int str)> options = new();
            //left
            {
                var newDir = (result.dir + 3) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.IsInGrid(newPoint))
                {
                    options[newPoint] = (result.dist + map[newPoint], newDir, 1);
                }
            }

            // right
            {
                var newDir = (result.dir + 1) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.IsInGrid(newPoint))
                {
                    options[newPoint] = (result.dist + map[newPoint], newDir, 1);
                }
            }

            // straight
            {
                var newPoint = result.loc + vectors[result.dir];
                if (map.IsInGrid(newPoint) && result.str < 3)
                {
                    options[newPoint] = (result.dist + map[newPoint], result.dir, result.str + 1);
                }
            }

            foreach (var pair in options.OrderByDescending(o => o.Key.MDistanceTo(goal)))
            {
                stack.Push((pair.Key, pair.Value.dist, pair.Value.dir, pair.Value.str));
            }
        }

        //memo[start] = min;
        return min;
    }

    public static long FindShortestPath2(Grid<int> map, Point start, Point goal)
    {
        var min = long.MaxValue;

        Dictionary<(Point, int, int), long> visited = new();
        Stack<(Point loc, long dist, int dir, int str)> stack = new([(start, 0, 1, 1)]);

        while (stack.TryPop(out var result))
        {
            if (result.dist + result.loc.MDistanceTo(goal) >= min) continue;
            if (result.loc == goal)
            {
                if (result.str >= 4)
                {
                    min = Math.Min(min, result.dist);
                }
                continue;
            }
            if (visited.TryGetValue((result.loc, result.dir, result.str), out var distance) && distance <= result.dist) continue;
            visited[(result.loc, result.dir, result.str)] = result.dist;

            Dictionary<Point, (long dist, int dir, int str)> options = new();
            //left
            {
                var newDir = (result.dir + 3) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.IsInGrid(newPoint) && result.str >= 4)
                {
                    options[newPoint] = (result.dist + map[newPoint], newDir, 1);
                }
            }

            // right
            {
                var newDir = (result.dir + 1) % 4;
                var newPoint = result.loc + vectors[newDir];
                if (map.IsInGrid(newPoint) && result.str >= 4)
                {
                    options[newPoint] = (result.dist + map[newPoint], newDir, 1);
                }
            }

            // straight
            {
                var newPoint = result.loc + vectors[result.dir];
                if (map.IsInGrid(newPoint) && result.str < 10)
                {
                    options[newPoint] = (result.dist + map[newPoint], result.dir, result.str + 1);
                }
            }

            foreach (var pair in options.OrderByDescending(o => o.Key.MDistanceTo(goal)))
            {
                stack.Push((pair.Key, pair.Value.dist, pair.Value.dir, pair.Value.str));
            }
        }

        //memo[start] = min;
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