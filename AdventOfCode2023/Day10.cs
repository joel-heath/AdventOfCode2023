using System;

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
            "TestInput1",
            "ExpectedOutput1"
        },

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
        var currentType = map[(current.Y,current.X)];
        var destinationType = map[(destination.Y,destination.X)];

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

    public string SolvePart2(string input)
    {



        return string.Empty;
    }
}