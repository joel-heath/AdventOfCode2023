namespace AdventOfCode2023;
public class Day10 : IDay
{
    public int Day => 10;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            ".....\r\n.S-7.\r\n.|.|.\r\n.L-J.\r\n.....",
            "4"
        },
        {
            "-L|F7\r\n7S-7|\r\nL|7||\r\n-L-J|\r\nL|-JF",
            "4"
        },
        {
            "..F7.\r\n.FJ|.\r\nSJ.L7\r\n|F--J\r\nLJ...",
            "8"
        },
        {
            "7-F7-\r\n.FJ|7\r\nSJLL7\r\n|F--J\r\nLJ.LJ",
            "8"
        }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "...........\r\n.S-------7.\r\n.|F-----7|.\r\n.||.....||.\r\n.||.....||.\r\n.|L-7.F-J|.\r\n.|..|.|..|.\r\n.L--J.L--J.\r\n...........",
            "4"
        },
        {
            ".F----7F7F7F7F-7....\r\n.|F--7||||||||FJ....\r\n.||.FJ||||||||L7....\r\nFJL7L7LJLJ||LJ.L-7..\r\nL--J.L7...LJS7F-7L7.\r\n....F-J..F7FJ|L7L7L7\r\n....L7.F7||L7|.L7L7|\r\n.....|FJLJ|FJ|F7|.LJ\r\n....FJL-7.||.||||...\r\n....L---J.LJ.LJLJ...",
            "8"
        },
        {
            "FF7FSF7F7F7F7F7F---7\r\nL|LJ||||||||||||F--J\r\nFL-7LJLJ||||||LJL-77\r\nF--JF--7||LJLJ7F7FJ-\r\nL---JF-JLJ.||-FJLJJ7\r\n|F|F-JF---7F7-L7L|7|\r\n|FFJF7L7F-JF7|JL---7\r\n7-L-JL7||F7|L7F-7F7|\r\nL.L7LFJ|||||FJL7||LJ\r\nL7JLJL-JLJLJL--JLJ.L",
            "10"
        }
    };

    static void DrawPoint(Point current, int distance)
    {
        var oldLcoation = Console.GetCursorPosition();
        Console.SetCursorPosition(current.X, current.Y);
        Console.Write(distance);
        Console.SetCursorPosition(oldLcoation.Left, oldLcoation.Top);
    }


    static int Dijkstras(Point start, Grid<char> map)
    {
        Dictionary<Point, int> visited = [];
        Queue<(Point, int)> toVisit = new([(start, 0)]);
        Dictionary<Point, int> workingValues = new();

        while (toVisit.TryDequeue(out var thing))
        {
            (Point current, int distance) = thing;
            visited.Add(current, distance);

            //DrawPoint(current, distance);

            var neighbours = map.Neighbours(current).Where(n => CanConnect(current, n, map)).Where(n => !visited.ContainsKey(n));
            var newDistance = distance + 1;
            foreach (var neighbor in neighbours)
            {
                if (workingValues.TryGetValue(neighbor, out var oldDistance))
                {
                    if (newDistance < oldDistance)
                    {
                        workingValues[neighbor] = newDistance;
                    }
                }
                else
                    workingValues.Add(neighbor, newDistance);
            }

            try
            {
                var nextNode = workingValues.Where(i => !visited.ContainsKey(i.Key)).MinBy(n => n.Value);
                toVisit.Enqueue((nextNode.Key, nextNode.Value));
            }
            catch { }
        }

        return visited.Values.Max();
    }

    static bool CanConnect(Point current, Point destination, Grid<char> map)
    {
        var currentType = map[(current.X, current.Y)];
        var destinationType = map[(destination.X, destination.Y)];

        bool north1 = false, south1 = false, east1 = false, west1 = false;
        bool north2 = false, south2 = false, east2 = false, west2 = false;

        if (currentType == '|') { north1 = true; south1 = true; }
        else if (currentType == '-') { east1 = true; west1 = true; }
        else if (currentType == 'L') { north1 = true; east1 = true; }
        else if (currentType == 'J') { north1 = true; west1 = true; }
        else if (currentType == '7') { south1 = true; west1 = true; }
        else if (currentType == 'F') { south1 = true; east1 = true; }
        else if (currentType == 'S') { north1 = true; east1 = true; south1 = true; west1 = true; }

        if (destinationType == '|') { north2 = true; south2 = true; }
        else if (destinationType == '-') { east2 = true; west2 = true; }
        else if (destinationType == 'L') { south2 = true; west2 = true; }
        else if (destinationType == 'J') { south2 = true; east2 = true; }
        else if (destinationType == '7') { north2 = true; east2 = true; }
        else if (destinationType == 'F') { north2 = true; west2 = true; }
        else if (destinationType == 'S') { north2 = true; east2 = true; south2 = true; west2 = true; } // S

        return (destination - current) switch
        {
            (0, -1) => north1 && north2,
            (0, 1) => south1 && south2,
            (1, 0) => east1 && east2,
            (-1, 0) => west1 && west2,
            _ => false
        };
    }

    public string SolvePart1(string input)
    {
        long total = 0;

        Point start = (0, 0);
        var lines = input.Split(Environment.NewLine);
        Grid<char> grid = new(lines.Length, lines[0].Length);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];


            for (int j = 0; j < line.Length; j++)
            {
                var c = line[j];
                grid[(i, j)] = c;

                if (c == 'S')
                    start = (i, j);
            }
        }

        var answer = Dijkstras(start, grid);


        return $"{answer}";
    }

    static HashSet<Point> Search(Point start, Grid<char> map)
    {
        HashSet<Point> visited = [];
        Queue<Point> toVisit = new([start]);
        HashSet<Point> workingValues = new();

        while (toVisit.TryDequeue(out Point current))
        {

            visited.Add(current);

            //DrawPoint(current, distance);

            var neighbours = map.Neighbours(current);

            //var neighbours = map.Neighbours(current).Where(n => CanConnect(current, n, map)).Where(n => !visited.Contains(n) && !toVisit.Contains(n));



            foreach (var neighbor in neighbours)
            {
                if (CanConnect(current, neighbor, map))
                {
                    if (!visited.Contains(neighbor))
                    {
                        if (!toVisit.Contains(neighbor))
                        {
                            toVisit.Enqueue(neighbor);
                        }
                    }
                }
            }
        }

        return visited;
    }

    static void DrawLoop(HashSet<Point> loop, Grid<char> grid)
    {
        Console.Clear();
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (loop.Contains((x, y)))
                {
                    Console.Write(grid[(x, y)]);
                    //Console.Write("O");
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.CursorTop++;
            Console.CursorLeft = 0;
        }



        /*
        foreach (var point in loop)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write('O');
        }*/
    }

    static void WriteLoop(HashSet<Point> loop, Grid<char> grid)
    {
        using StreamWriter writer = new(new FileStream("day10", FileMode.Create));

        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                if (loop.Contains((x, y)))
                {
                    writer.Write(grid[(x, y)]);
                    //Console.Write("O");
                }
                else
                {
                    writer.Write('.');
                }
            }
            writer.WriteLine();
        }



        /*
        foreach (var point in loop)
        {
            Console.SetCursorPosition(point.X, point.Y);
            Console.Write('O');
        }*/
    }

    public static bool NewWall(Point point, int direction, Grid<char> map)
    {
        char symbol = map[point];
        if (direction == 0 || direction == 2)
        {
            return symbol != '|';
        }
        return symbol != '-';
    }

    static char DetermineS(Grid<char> grid, Point s)
    {
        var adjacents = new Point[] { (0, -1), (1, 0), (0, 1), (-1, 0) }
            .ToDictionary(p => p, p => grid[s + p]);



        bool north = false, south = false, east = false, west = false;

        if (adjacents[(0, -1)] == '|' || adjacents[(0, -1)] == 'F' || adjacents[(0, -1)] == '7') { north = true; }
        if (adjacents[(0, 1)] == '|' || adjacents[(0, 1)] == 'L' || adjacents[(0, 1)] == 'J') { south = true; }

        if (adjacents[(1, 0)] == '-' || adjacents[(1, 0)] == 'J' || adjacents[(1, 0)] == '7') { east = true; }
        if (adjacents[(-1, 0)] == '-' || adjacents[(-1, 0)] == 'L' || adjacents[(-1, 0)] == 'F') { west = true; }

        if (north && south && !east && !west) return '|';
        if (!north && !south && east && west) return '-';
        if (north && !south && east && !west) return 'L';
        if (north && !south && !east && west) return 'J';
        if (!north && south && east && !west) return 'F';
        if (!north && south && !east && west) return '7';

        throw new Exception("Cannot determine starting pipe");
    }

    static (Grid<char>, Point) ParseInput(string input)
    {
        var rawLines = input.Split(Environment.NewLine);
        var lines = rawLines.Select(l => string.Concat(new List<char> { '.' }.Concat(l).Append('.'))).Prepend(new string('.', rawLines[0].Length + 2)).Append(new string('.', rawLines[0].Length + 2)).ToArray();

        var grid = new Grid<char>(lines[0].Length, lines.Length);

        Point start = (0, 0);

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];


            for (int x = 0; x < line.Length; x++)
            {
                var c = line[x];
                grid[(x, y)] = c;

                if (c == 'S')
                    start = (x, y);
            }
        }

        // change S appropraitely
        grid[start] = DetermineS(grid, start);

        return (grid, start);
    }

    public string SolvePart2(string input)
    {
        var (grid, start) = ParseInput(input);

        var loop = Search(start, grid);


        int count = 0;
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var here = (x, y);
                if (loop.Contains(here)) continue;

                bool inGrid = false;
                for (int i = 0; i < 4; i++)
                {
                    var pointsToCheck = grid.LineOut(here, i, false);
                    var walls = 0;
                    var lastCorner = (char)0;
                    foreach (var point in pointsToCheck)
                    {
                        if (loop.Contains(point))
                        {
                            var symbol = grid[point];
                            var isNewWall = NewWall(point, i, grid);

                            if (isNewWall)
                            {
                                if (lastCorner != (char)0)
                                {
                                    if (i == 1 || i == 3)
                                    {
                                        if (lastCorner == 'F' && symbol == '7' || lastCorner == '7' && symbol == 'F'
                                         || lastCorner == 'L' && symbol == 'J' || lastCorner == 'J' && symbol == 'L') walls++;
                                    }
                                    else
                                    {
                                        if (lastCorner == 'F' && symbol == 'L' || lastCorner == 'L' && symbol == 'F'
                                         || lastCorner == '7' && symbol == 'J' || lastCorner == 'J' && symbol == '7') walls++;
                                    }

                                    lastCorner = (char)0;
                                }
                                else
                                {
                                    if (symbol != '-' && symbol != '|') lastCorner = symbol;
                                    walls++;
                                }
                            }
                        }
                    }
                    //if (wall) walls++;

                    if (walls % 2 == 1)
                    {
                        //Console.WriteLine(here);
                        inGrid = true;
                        //Console.WriteLine((x, y));
                        break;
                    }
                }

                if (inGrid)
                {
                    count++;
                }
            }
        }

        return $"{count}";

    }

        /*
        public string SolvePart2(string input)
        {
            Point start = (0, 0);
            var lines = input.Split(Environment.NewLine);
            Grid<char> grid = new(lines[0].Length, lines.Length);
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];


                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    grid[(x, y)] = c;

                    if (c == 'S')
                        start = (x, y);
                }
            }

            var loop = Search(start, grid);


            WriteLoop(loop, grid);
            //DrawLoop(loop, grid);

            int count = 0;
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    var here = (x, y);
                    if (grid[here] != '.') continue;

                    bool inGrid = true;
                    for (int i = 0; i < 4; i++)
                    {
                        var pointsToCheck = grid.LineOut(here, i, false);
                        var wall = false;
                        bool polarity = true; // if %2 == 1 wall, then inclusive
                        var walls = 0;
                        foreach (var point in pointsToCheck)
                        {
                            if (loop.Contains(point))
                            {
                                if (grid[point] == (i % 2 == 0 ? '-' : '|')) polarity = !polarity;
                                else if (!wall)
                                {
                                    walls++;
                                }
                                wall = true;
                            }
                            else
                            {
                                if (wall)
                                {
                                    wall = false;
                                }
                            }
                        }

                        if (walls % 2 != (polarity ? 1 : 0))
                        {
                            inGrid = false;
                            break;
                        }
                    }

                    if (inGrid)
                    {
                        count++;
                    }
                }
            }

            return $"{count}";
        }
        */
        /*
        public string SolvePart2(string input)

        {
            Point start = (0, 0);
            var lines = input.Split(Environment.NewLine);
            Grid<char> grid = new(lines[0].Length * 2, lines.Length * 2);
            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    grid[(x, y)] = c;

                    if (c == 'S')
                        start = (x, y);
                }
            }

            var loop = Search(start, grid);


            var inners = new HashSet<Point>();
            var outers = new HashSet<Point>();

            foreach (var point in loop)
            {
                var symbol = grid[point];

                point.


            }


            return "";
        }
        */

        /*
        Queue<Point> remaining = new([(0, 0)]);

        HashSet<Point> visited = new();
        while (remaining.TryDequeue(out var curr))
        {
            visited.Add(curr);

            foreach (var neighbour in grid.Neighbours(curr).Where(n => !visited.Contains(n) && !remaining.Contains(n)))
            {
                remaining.Enqueue(neighbour);
            }


        }



        return $"{0}";
    }
        */




        /*
        var floodQueue = Enumerable.Range(0, grid.Width - 1).Select(i => (i, 0)).Concat(Enumerable.Range(0, grid.Height - 1).Select(i => (grid.Width - 1, i)))
                 .Concat(Enumerable.Range(1, grid.Width - 1).Select(i => (i, grid.Height - 1)).Concat(Enumerable.Range(1, grid.Height - 1).Select(i => (0, i))))
                 .Select(i => !loop.Contains(i));
        */

        /*
        int count = 0;
        for (int y = 0; y < grid.Height; y++)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                var here = (x, y);
                if (grid[here] != '.') continue;

                bool inGrid = true;
                for (int i = 0; i < 4; i++)
                {
                    var pointsToCheck = grid.LineOut(here, i, false);
                    var wall = false;
                    bool wallDir = false;
                    var walls = 0;
                    foreach (var point in pointsToCheck)
                    {
                        if (loop.Contains(point))
                        {
                            if (wall == true)
                            {
                                if (wallDir != (wallDir = DetermineWallDirection(point, i, grid)))
                                    walls++;
                            }
                            else
                            {
                                wall = true;
                                wallDir = DetermineWallDirection(point, i, grid);
                                walls++;
                            }
                        }
                        else
                        {
                            if (wall)
                            {
                                wall = false;
                            }
                        }
                    }
                    //if (wall) switches++;

                    if (walls % 2 == 0)
                    {
                        inGrid = false;
                        break;
                    }
                }

                if (inGrid)
                {
                    count++;
                }
            }
        }*/


        /*
        int count = 0;
        bool vWall = false;
        for (int y = 0; y < grid.Height; y++)
        {
            bool enclosed = false;
            bool wall = false;
            int localCount = 0;
            for (int x = 0; x < grid.Width; x++)
            {
                if (loop.Contains((x, y)))
                {
                    if (wall == false && vWall == false)
                        count += localCount;
                    wall = true;
                    vWall = true;
                }
                else
                {
                    if (wall && vWall)
                    { 
                        enclosed = !enclosed;
                        localCount++;
                    }
                    else if (enclosed)
                    {
                        localCount++;
                    }
                    wall = false;
                    vWall = false;
                }
            }
        }*/
    }