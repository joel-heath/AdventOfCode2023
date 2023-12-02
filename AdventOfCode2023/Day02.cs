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
                .Select((l, i) => (l.Split(' ')[2..]
                    .Chunk(2)
                    .All(c => c[1].Contains('d') ? int.Parse(c[0]) <= 12 : c[1].Contains('g') ? int.Parse(c[0]) <= 13 : int.Parse(c[0]) <= 14), i + 1))
                .Where(g => g.Item1)
                .Sum(g => g.Item2)}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine)
                    .Select(l => l.Split(' ')[2..]
                        .Chunk(2)
                        .GroupBy(i => i[1].TrimEnd(',').TrimEnd(';'))
                        .Select(g => g.Max(t => int.Parse(t[0])))
                        .Aggregate((acc, i) => acc * i))
                    .Sum()}";
}