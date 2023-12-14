namespace AdventOfCode2023;
public class Day14 : IDay
{
    public int Day => 14;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....",
            "136"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....",
            "64"
        },

    };

    public static Point Slide(Point location, Grid<char> map, int direction = 0)
    {
        var newLocation = map.LineOut(location, 0, false).TakeWhile(c => map[c] == '.').LastOrDefault(location);

        map[location] = '.';
        map[newLocation] = 'O';

        return newLocation;
    }

    public string SolvePart1(string input)
    {
        long total = 0;

        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if (grid[(x, y)] == 'O')
                {
                    var newY = Slide((x, y), grid).Y;

                    total += (grid.Height - newY);
                }
            }
        }

        return $"{total}";
    }

    static int FindRepeat(List<Point[]> history, Point[] current)
    {
        for (int i = 0; i < history.Count; i++)
        {
            bool match = true;
            var arr = history[i];
            for (int j = 0; j < arr.Length; j++)
            {
                if (current[j] != arr[j])
                { match = false; break; }
            }

            if (match)
                return i;
        }

        return -1;
    }

    public string SolvePart2(string input)
    {
        long total = 0;

        var points = input.Split(Environment.NewLine).SelectMany((l, i) => l.Select((c, ci) => (new Point(ci, i), c))).ToArray();
        var roundRocks = points.Where(p => p.c == 'O').Select(p => p.Item1).ToArray();
        var squareRocks = points.Where(p => p.c == '#').Select(p => p.Item1).ToArray();
        Point bounds = (points.Max(p => p.Item1.X), points.Max(p => p.Item1.Y));

        const int COUNT = 1_000_000_000;
        //const int COUNT = 1;

        var history = new List<Point[]>();
        int i = 0;
        int lb = -1;
        for (; i < COUNT; i++)
        {
            for (int c = 0; c < 4; c++)
            {
                roundRocks = c == 0
                    ? roundRocks.OrderBy(r => r.Y).ToArray()
                    : c == 1
                        ? roundRocks.OrderBy(r => r.X).ToArray()
                        : c == 2
                            ? roundRocks.OrderByDescending(r => r.Y).ToArray()
                            : roundRocks.OrderByDescending(r => r.X).ToArray();

                for (int j = 0; j < roundRocks.Length; j++)
                {
                    var newLocation = FindNewLocation(roundRocks[j], c, roundRocks, squareRocks, bounds);
                    roundRocks[j] = newLocation;
                }
            }

            lb = FindRepeat(history, roundRocks);
            if (lb > -1) break;

            history.Add(roundRocks);
        }

        var remaining = COUNT - i - 1;
        var repeatingGroup = history[lb..];

        roundRocks = repeatingGroup[remaining % repeatingGroup.Count];

        

        return $"{roundRocks.Sum(r => bounds.Y + 1 - r.Y)}";
    }

    public static void VisualiseRocks(Point[] round, Point[] square, Point bounds)
    {
        var (left, top) = Console.GetCursorPosition();

        for (int y = 0; y <= bounds.Y; y++)
        {
            for (int x = 0; x <= bounds.X; x++)
            {
                Console.SetCursorPosition(left + x, top + y);
                Console.Write('.');
            }
        }

        foreach (var item in round)
        {
            Console.SetCursorPosition(left + (int)item.X, top + (int)item.Y);
            Console.Write('O');
        }
        foreach (var item in square)
        {
            Console.SetCursorPosition(left + (int)item.X, top + (int)item.Y);
            Console.Write('#');
        }
        Console.SetCursorPosition(left, top + (int)bounds.Y + 2);
    }

    public static Point FindNewLocation(Point start, int direction, Point[] round, Point[] square, Point bounds)
    {
        var dVector = direction == 0 ? (0, -1) : direction == 1 ? (-1, 0) : direction == 2 ? (0, 1) : (1, 0);

        var location = start;
        while (true)
        {
            var newLocation = location + dVector;
            if (square.Contains(newLocation) || round.Contains(newLocation)
                || newLocation.X > bounds.X || newLocation.Y > bounds.Y
                || newLocation.X < 0 || newLocation.Y < 0) break;
            location = newLocation;
        }

        return location;
    }
}