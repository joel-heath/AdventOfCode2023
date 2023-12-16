namespace AdventOfCode2023;
public class Day16 : IDay
{
    public int Day => 16;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            ".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....",
            "46"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            ".|...\\....\r\n|.-.\\.....\r\n.....|-...\r\n........|.\r\n..........\r\n.........\\\r\n..../.\\\\..\r\n.-.-/..|..\r\n.|....-|.\\\r\n..//.|....",
            "51"
        },

    };

    private static readonly Point[] vectors = [(0, -1), (1, 0), (0, 1), (-1, 0)];

    private static HashSet<(Point, int)> memo = [];

    public HashSet<Point> TrackBeam(Grid<char> map, Point location, int direction, HashSet<Point>? points = null)
    {
        points ??= [];
        if (memo.Contains((location, direction))) return points;

        while (map.IsInGrid(location))
        {
            points.Add(location);
            var symbol = map[location];

            //(location).Dump();
            memo.Add((location, direction));

            if (symbol == '\\' || symbol == '/')
            {
                if (direction == 0)
                {
                    if (symbol == '\\') direction = 3;
                    else direction = 1;
                }
                else if (direction == 1)
                {
                    if (symbol == '\\') direction = 2;
                    else direction = 0;
                }
                else if (direction == 2)
                {
                    if (symbol == '\\') direction = 1;
                    else direction = 3;
                }
                else if (direction == 3)
                {
                    if (symbol == '\\') direction = 0;
                    else direction = 2;
                }
            }
            else if (symbol == '|' || symbol == '-')
            {
                if (!(((direction == 0 || direction == 2) && symbol == '|')
                    || ((direction == 1 || direction == 3) && symbol == '-')))
                {
                    if (symbol == '|')
                    {
                        TrackBeam(map, location + (0, -1), 0, points);
                        TrackBeam(map, location + (0, 1), 2, points);
                    }
                    else
                    {
                        TrackBeam(map, location + (1, 0), 1, points);
                        TrackBeam(map, location + (-1, 0), 3, points);
                    }
                    return points;
                }
            }

            location += vectors[direction];
        }

        return points;
    }

    public string SolvePart1(string input)
    {
        long total = 0;
        memo = [];
        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());

        return $"{TrackBeam(grid, (0, 0), 1).Count}";
    }

    public string SolvePart2(string input)
    {
        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());

        int max = 0;
        for (int i = 0; i < grid.Height; i++)
        {
            memo = [];
            max = Math.Max(max, TrackBeam(grid, (i, 0), 2).Count);
            memo = [];
            max = Math.Max(max, TrackBeam(grid, (i, grid.Height - 1), 0).Count);
        }
        for (int i = 0; i < grid.Width; i++)
        {

            memo = [];
            max = Math.Max(max, TrackBeam(grid, (0, i), 1).Count);
            memo = [];
            max = Math.Max(max, TrackBeam(grid, (grid.Width - 1, i), 3).Count);
        }

        return $"{max}";
    }
}