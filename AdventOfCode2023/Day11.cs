using System.Diagnostics;

namespace AdventOfCode2023;
public class Day11 : IDay
{
    public int Day => 11;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "...#......\r\n.......#..\r\n#.........\r\n..........\r\n......#...\r\n.#........\r\n.........#\r\n..........\r\n.......#..\r\n#...#.....", "374" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
    };



    public string SolvePart1(string input)
    {
        long total = 0;

        var lines = input.Split(Environment.NewLine);
        List<char[]> rowsExpanded = new();

        var galaxies = new List<Point>();

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            var row = line.ToCharArray();

            bool containsGalaxies = false;
            for (int x = 0; x < line.Length; x++)
            {
                var symbol = line[x];
                if (symbol == '#')
                {
                    containsGalaxies = true;
                }
            }

            if (!containsGalaxies)
            {
                rowsExpanded.Add(row);
            }
            rowsExpanded.Add(row);
        }

        //rowsExpanded.Dump();

        List<char[]> colsExpanded = new List<char[]>();

        for (int x = 0; x < rowsExpanded[0].Length; x++)
        {
            var col = new char[rowsExpanded.Count];
            bool galaxy = false;
            for (int y = 0; y < rowsExpanded.Count; y++)
            {
                col[y] = rowsExpanded[y][x];
                if (rowsExpanded[y][x] == '#')
                    galaxy = true;
            }
            if (!galaxy)
                colsExpanded.Add(col);
            colsExpanded.Add(col);
        }

        //colsExpanded.Dump();

        Grid<char> map = new Grid<char>(colsExpanded.ToArray(), false);

        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map[x, y] == '#')
                    galaxies.Add((x, y));
            }
        }

        Dictionary<Point, long> pairs = new();

        long sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            for (int j = 0; j < galaxies.Count; j++)
            {
                if (i == j) continue;
                if (pairs.ContainsKey((i, j))) continue;
                if (pairs.ContainsKey((j, i))) continue;

                //var distance = galaxies[i].MDistanceTo(galaxies[j]);

                var distance = Math.Abs(galaxies[j].Y - galaxies[i].Y) + Math.Abs(galaxies[j].X - galaxies[i].X);

                //(j, i, distance).Dump();

                pairs[(i, j)] = distance;
                sum += distance;
            }
        }
        pairs.Count.Dump();

        return $"{sum}";
    }

    static Point GetPos(Point init, IReadOnlyList<long> emptyRows, IReadOnlyList<long> emptyCols, long expansionFactor)
    {
        return (emptyCols.TakeWhile(c => c < init.X).Count() * (expansionFactor - 1) + init.X, emptyRows.TakeWhile(r => r < init.Y).Count() * (expansionFactor - 1) + init.Y);
    }

    public string SolvePart2(string input)
    {
        long total = 0;

        var lines = input.Split(Environment.NewLine);

        var galaxies = new List<Point>();
        List<long> emptyRows = [];
        List<long> emptyCols = [];

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];

            bool rowContainsGalaxies = false;
            for (int x = 0; x < line.Length; x++)
            {
                if (line[x] == '#')
                {
                    rowContainsGalaxies = true;
                    galaxies.Add((x, y));
                }
            }
            if (!rowContainsGalaxies) emptyRows.Add(y);
        }

        for (int x = 0; x < lines.Length; x++)
        {
            bool colContainsGalaxies = false;
            for (int y = 0; y < lines[0].Length; y++)
            {
                if (lines[y][x] == '#') colContainsGalaxies = true;
            }
            if (!colContainsGalaxies) emptyCols.Add(x);
        }

        Dictionary<Point, long> pairs = [];

        long sum = 0;
        for (int i = 0; i < galaxies.Count; i++)
        {
            var here = GetPos(galaxies[i], emptyRows, emptyCols, 1000000);
            for (int j = 0; j < galaxies.Count; j++)
            {
                var there = GetPos(galaxies[j], emptyRows, emptyCols, 1000000);
                if (i == j) continue;
                if (pairs.ContainsKey((i, j))) continue;
                if (pairs.ContainsKey((j, i))) continue;

                var distance = Math.Abs(there.Y - here.Y) + Math.Abs(there.X - here.X);

                pairs[(i, j)] = distance;
                sum += distance;
            }
        }
        pairs.Count.Dump();

        return $"{sum}";
    }
}