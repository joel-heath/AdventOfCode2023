namespace AdventOfCode2023;
public class Day04 : IDay
{
    public int Day => 4;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", "13" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53\r\nCard 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19\r\nCard 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1\r\nCard 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83\r\nCard 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36\r\nCard 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", "30" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Select(l =>
            l.Split(" | ").Select(i => i.Split(": ").Last()).Select(s => s.Split(' ').Where(i => i != string.Empty)).ToArray())
            .Sum(l => (int)Math.Pow(2, l[0].Intersect(l[1]).Count() - 1))}";

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var instances = Enumerable.Range(0, lines.Length).ToDictionary(i => i, i => 1);
        return $"{lines
            .Select(l => l.Split(" | ").Select(i => i.Split(": ").Last()).Select(s => s.Split(' ').Select(e => e.Trim()).Where(i => i != " " && i != "")).ToArray())
            .Select(l => l[0].Intersect(l[1]).Count())
            .Select((l, i) => {
                for (int j = 0; j < instances[i]; j++)
                {
                    for (int k = 1; k <= l; k++)
                        instances[i + k]++;
                }
                return l;
            })
            .SelectMany((l, i) => Enumerable.Repeat(l, instances[i])).Count()}";
    }
}
