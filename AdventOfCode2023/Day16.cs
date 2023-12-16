namespace AdventOfCode2023;
public class Day16 : IDay
{
    public int Day => 16;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { ".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....", "46" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { ".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....", "51" }
    };

    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    public string SolvePart1(string input)
        => $"{TrackBeam(new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()), (0, 0), 1).Count}";

    public string SolvePart2(string input)
        => $"{new Grid<char>[] { new(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()) }.Select(grid => 
            Enumerable.Range(0, grid.Height).SelectMany(i => new (int x, int y, int d)[] { (i, 0, 2), (i, grid.Height - 1, 0) })
            .Concat(Enumerable.Range(0, grid.Width).SelectMany(i => new (int x, int y, int d)[] { (0, i, 1), (grid.Width - 1, i, 3) }))
            .Max(i => TrackBeam(grid, (i.x, i.y), i.d).Count)).First()}";

    public HashSet<Point> TrackBeam(Grid<char> map, Point location, int direction, HashSet<Point>? points = null, HashSet<(Point, int)>? memo = null)
    => new (HashSet<Point>, HashSet<(Point, int)>)[] { (points ??= [], memo ??= []) }.Select(_ =>
        memo.Contains((location, direction)) || !map.IsInGrid(location) ? points :
            Utils.EnumerateForever(new object()).TakeWhile(_ =>
                new char[] { points.Add(location) && false || memo.Add((location, direction)) ? map[location] : map[location] }.Select(symbol =>
                    symbol == '\\' || symbol == '/'
                        ? (direction = symbol == '\\' ? direction = 4 - direction - 1 : direction + (int)Math.Pow(-1, direction)) == -1 || true
                        : (symbol != '|' || direction % 2 != 1) && (symbol != '-' || direction % 2 != 0)
                            || (symbol == '|'
                                ? TrackBeam(map, location + (0, -1), 0, points, memo).Count == -1 ||
                                   TrackBeam(map, location + (0, 1), 2, points, memo).Count == -1
                                : TrackBeam(map, location + (1, 0), 1, points, memo).Count == -1 ||
                                   TrackBeam(map, location + (-1, 0), 3, points, memo).Count == -1))
                    .Select(b => b && (location += vectors[direction]) != (-1, -1))
                    .First() && map.IsInGrid(location)
                    ).ToArray().Length == 1 ? points : points).First();
}