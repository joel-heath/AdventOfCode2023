using System;

namespace AdventOfCode2023;
public class Day03 : IDay
{
    public int Day => 3;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "4361" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "467835" }

    };

    public static IEnumerable<(int, int)> GetAllAdjacent(string[] input, int row, int index, int length)
        => Enumerable.Range(0, length).Select(i => (row - 1, index + i)).Concat(Enumerable.Range(0, length).Select(i => (row + 1, index + i)))
            .Concat(Enumerable.Range(-1, 3).Select(i => (row + i, index - 1))).Concat(Enumerable.Range(-1, 3).Select(i => (row + i, index + length)))
            .Where(c => c.Item1 >= 0 && c.Item1 < input.Length && c.Item2 >= 0 && c.Item2 < input[c.Item1].Length);

    public static (string left, string right) FindNum(string row, int index)
    => (string.Concat(row[..index].Reverse().TakeWhile(c => '0' <= c && c <= '9').Reverse()), string.Concat(row[(index + 1)..].TakeWhile(c => '0' <= c && c <= '9')));

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        return $"{lines.Select((l, i) =>
            Utils.FindAll(@"\d+", l)
                .Where(n => GetAllAdjacent(lines, i, n.Index, n.Length)
                    .Select(c => lines[c.Item1][c.Item2])
                    .Any(i => i != '.' && (i < '0' || i > '9'))
                ).Sum(n => int.Parse(n.Value))
        ).Sum()}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        int total = lines.Select((l, i) =>
            Utils.FindAll(@"\*", l)
                .Select(g => GetAllAdjacent(lines, i, g.Index, 1)
                    .Where(i => lines[i.Item1][i.Item2] >= '0' && lines[i.Item1][i.Item2] <= '9')
                //                                  rowIndex  colIndex       (12, 45) given the lines[Item1,Item2] = 12345
                    .Select(n => (n.Item1, n.Item2, FindNum(lines[n.Item1], n.Item2)))
                    //           rowIndex, colIndex,    12,            45,            index of 1 in 12               index of 5 in 45
                    .Select(t => (t.Item1, t.Item2, t.Item3.left, t.Item3.right, t.Item2 - t.Item3.left.Length, t.Item2 + t.Item3.right.Length))
                    .DistinctBy(t => (t.Item1, t.Item5, t.Item6)) // distinct by indices of 1 and 5, the full number AND it's row
                    .Select(t => t.left + lines[t.Item1][t.Item2] + t.right) // create full number now, 12345
                    .Select(int.Parse)
                    .ToArray())
                .Where(g => g.Length == 2)
                .Sum(g => g.Aggregate((acc, v) => acc * v))
            ).Sum();

        return $"{total}";
    }
}
