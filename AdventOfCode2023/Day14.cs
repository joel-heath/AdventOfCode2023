using System;

namespace AdventOfCode2023;
public class Day14 : IDay
{
    public int Day => 14;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....", "136" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "O....#....\r\nO.OO#....#\r\n.....##...\r\nOO.#O....O\r\n.O.....O#.\r\nO.#..O.#.#\r\n..O..#O..O\r\n.......O..\r\n#....###..\r\n#OO..#....", "64" }
    };

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

    public string SolvePart2(string input)
    {
        long total = 0;

        var grid = new Grid<char>(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray());

        List<string> history = [];
        int cycles = 1_000_000_000;
        int lb = -1;
        for ( ; cycles > 0; cycles--)
        {
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[(x, y)] == 'O')
                    {
                        Slide((x, y), grid, 0);
                    }
                }
            }
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[(x, y)] == 'O')
                    {
                        Slide((x, y), grid, 3);
                    }
                }
            }
            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = grid.Height - 1; y >= 0; y--)
                {
                    if (grid[(x, y)] == 'O')
                    {
                        Slide((x, y), grid, 2);
                    }
                }
            }
            for (int x = grid.Width - 1; x >= 0; x--)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    if (grid[(x, y)] == 'O')
                    {
                        Slide((x, y), grid, 1);
                    }
                }
            }

            var locations = grid.ToString();
            lb = history.IndexOf(locations);
            if (lb > -1) break;
            history.Add(locations);
        }

        var cycle = history[lb..];
        var finalGrid = cycle[(cycles - 1) % cycle.Count];

        return $"{finalGrid.Split(Environment.NewLine).Reverse().SelectMany((l, i) => l.Select(c => (c, i))).Where(c => c.c == 'O').Sum(r => r.i).Dump()}";
    }

    public static Point Slide(Point location, Grid<char> map, int direction = 0)
    {
        var newLocation = map.LineOut(location, direction, false).TakeWhile(c => map[c] == '.').LastOrDefault(location);

        map[location] = '.';
        map[newLocation] = 'O';

        return newLocation;
    }
}