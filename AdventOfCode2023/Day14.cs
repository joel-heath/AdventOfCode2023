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
    => $"{new Grid<char>[] { new(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()) }
        .Select(grid =>
            Enumerable.Range(0, grid.Height).SelectMany(y => Enumerable.Range(0, grid.Width).Select(x => new Point(x, y)))
            .Where(p => grid[p] == 'O')
            .Sum(p => grid[p] == 'O' ? grid.Height - 
                new Point[] { ((grid[p] = '.') == '.') ? grid.LineOut(p, 0, false).TakeWhile(c => grid[c] == '.').LastOrDefault(p) : default }
                    .Select(p => ((grid[p] = 'O') == 'O') ? p.Y : 0).First() : 0)).First()}";

    public string SolvePart2(string input)
        => $"{new Grid<char>[] { new(input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray()) }
            .Select(grid => (grid, Enumerable.Range(0, 4).Select(
                c => new IEnumerable<int>[] { Enumerable.Range(0, grid.Width) }.Select(r => c == 3 ? r.Reverse() : r).First()
                    .SelectMany(x => new IEnumerable<Point>[] { Enumerable.Range(0, grid.Height).Select(y => new Point(x, y)) }.Select(r => c == 2 ? r.Reverse() : r).First()).ToArray()).ToArray()))
            .Select(input => 
        Utils.Range(1_000_000_000 - 1, -1_000_000_000).AggregateWhile<(Grid<char> grid, int lb, List<string> history, int i), int>
            ((input.grid, -1, new List<string>(), 0), (acc, i) =>
            new Grid<char>[] { input.Item2.Select((p, c) => p
                    .Select(p => acc.grid[p] == 'O' ? (acc.grid[p] = '.', acc.grid[acc.grid.LineOut(p, c % 2 == 1 ? 4 - c : c, false).TakeWhile(c => acc.grid[c] == '.').LastOrDefault(p)] = 'O') : (0,0))).Where(x => x.ToArray().Length == 2).ToArray().Length == 2 ? acc.grid : acc.grid
            }.Select(actualGrid => (actualGrid, acc.history.IndexOf(actualGrid.ToString()), acc.history.Append(actualGrid.ToString()).ToList(), i)).First()
            , acc => acc.lb == -1)).Select(results => (results.history[results.lb..^1], results.i)).Select(results => results.Item1[results.i % results.Item1.Count])
            .First().Split(Environment.NewLine).Reverse().SelectMany((l, i) => l.Select(c => (c, i))).Where(c => c.c == 'O').Sum(r => r.i)}";
}