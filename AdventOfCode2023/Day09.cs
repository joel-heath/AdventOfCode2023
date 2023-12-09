using System.Linq;
using System.Net.Sockets;

namespace AdventOfCode2023;
public class Day09 : IDay
{
    public int Day => 9;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45", "114" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45", "2" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Select(l =>
                new long[][] { l.Split(' ').Select(long.Parse).ToArray() }
                .Select(n =>
                    Enumerable.Range(0, n.Length)
                        .Aggregate(new long[][] { n }, (r, l)
                            => r.Append(r[l].Select((n, i) => (n, i)).Aggregate(new long[r[l].Length], (d, n) => (d[n.i] = n.n - (n.i > 0 ? r[l][n.i - 1] : 0)) == 0 ? d : d).Skip(1).ToArray()).ToArray())
                        .TakeWhile(n => n.Length > 0).Sum(t => t.Length > 0 ? t[^1] : 0))
                .First())
            .Sum()}";


    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine).Select(l =>
                new long[][] { l.Split(' ').Select(long.Parse).ToArray() }.Select(n =>
                    Enumerable.Range(0, n.Length)
                        .Aggregate(new long[][] { n }, (r, l)
                            => r.Append(r[l].Select((n, i) => (n, i)).Aggregate(new long[r[l].Length], (d, n) => (d[n.i] = n.n - (n.i > 0 ? r[l][n.i - 1] : 0)) == 0 ? d : d).Skip(1).ToArray()).ToArray())
                        .TakeWhile(n => n.Length > 0)
                        .Reverse().Aggregate(0L, (acc, curr) => curr[0] - acc))
                    .First())
            .Sum()}";
}