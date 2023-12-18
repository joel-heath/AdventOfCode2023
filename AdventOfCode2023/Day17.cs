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

    public string SolvePart1(string input) => $"{Solve(input, true)}";

    public string SolvePart2(string input) => $"{Solve(input, false)}";

    public static long Solve(string input, bool partone)
        => new Grid<int>[] { new(input.Split(Environment.NewLine).Select(l => l.Select(i => int.Parse($"{i}")).ToArray()).ToArray()) }
            .Select(map => 
        new (HashSet<(Point, int, int)> visited, PriorityQueue<(Point loc, long dist, int dir, int str), long> queue)[] { ([], new([(((0, 0), 0, 1, 1), 0)])) }.Select(s => 
        Utils.EnumerateForever().Select(_ => s.queue.Dequeue())
            .AggregateWhile(long.MaxValue, (min, result) =>
                result.dist + result.loc.MDistanceTo((map.Width - 1, map.Height - 1)) >= min ? min :
                result.loc == (map.Width - 1, map.Height - 1)
                    ? result.str >= 4 || partone ? Math.Min(min, result.dist) : min
                    : s.visited.Contains((result.loc, result.dir, result.str)) ? min :
            s.visited.Add((result.loc, result.dir, result.str)) &&
                (result.str < 4 && !partone ||
                new int[] { (result.dir + 3) % 4, (result.dir + 1) % 4 }.Select(newDir => (newDir, newPoint: result.loc + vectors[newDir]))
                    .Select(d => !map.Contains(d.newPoint) || new Action(() => s.queue.Enqueue((d.newPoint, result.dist + map[d.newPoint], d.newDir, 1), result.dist)).InvokeTruthfully()).All(b => b)) &&
                 result.str >= (partone ? 3 : 10) || new Point[] { result.loc + vectors[result.dir] }
                    .Select(d => !map.Contains(d) || new Action(() => s.queue.Enqueue((d, result.dist + map[d], result.dir, result.str + 1), result.dist)).InvokeTruthfully()).First()
                ? min : min, _ => s.queue.Count > 0)).First()).First();
}