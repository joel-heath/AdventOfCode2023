using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Runtime.Intrinsics.Arm;
using System.Text.RegularExpressions;
using static AdventOfCode2023.Utils;

namespace AdventOfCode2023;
public class Day02 : IDay
{
    public int Day => 2;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", "8" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", "2286" }
    }; 
    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine)
                   .Select(g => g.Split(": ")[1]
                       .Split("; ")
                       .SelectMany(s => s.Split(", ")
                           .Select(d => d.Split(' '))
                           .GroupBy(i => i[1]) // group by colours
                           .Select(g => g.ToArray()) // iterating twice + indexing, so need to ToArray() this
                           .Select(s => (s[0][1][0], s.Sum(i => int.Parse($"{i[0]}")))) // now we have (colour, count)
                           .Select(c =>
                               c.Item1 == 'r'
                                   ? c.Item2 <= 12 :
                               c.Item1 == 'g'
                                   ? c.Item2 <= 13 :
                                     c.Item2 <= 14
                           )) // now we have if each colour succedeed
                       .All(s => s)) // if each set succeeded
                   .Select((b, i) => (b, i + 1)) // (gameSucceeded?, gameID)
                   .Where(g => g.b) // now only have games that succeeded
                   .Sum(g => g.Item2)}"; // sum IDs

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine)
                   .Select(g => g.Split(": ")[1]
                       .Split("; ")
                       .SelectMany(s => s.Split(", ")
                           .Select(d => d.Split(' '))
                           .GroupBy(i => i[1])
                           .Select(g => g.ToArray())
                           .Select(s => (s[0][1], s.Sum(i => int.Parse($"{i[0]}")))))
                       .GroupBy(i => i.Item1)
                           .Select(g => g.Max(t => t.Item2))
                       .Aggregate((acc, i) => acc * i)
                   ).Sum()}";
}
