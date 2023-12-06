namespace AdventOfCode2023;
public class Day06 : IDay
{
    public int Day => 6;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "Time:      7  15   30\r\nDistance:  9  40  200", "288" },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Time:      7  15   30\r\nDistance:  9  40  200", "71503" },
    };

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine).Select(l => l.Split(' ').Skip(1).Where(c => c != string.Empty).Select(int.Parse).ToArray()).ToArray();

        var product = 1;
        for (int i = 0; i < lines[0].Length; i++)
        {
            var time = lines[0][i];
            var best = lines[1][i];

            int count = 0;
            for (int j = 0; j <= time; j++)
            {
                var timeMoving = time - j;
                var distance = j * timeMoving;

                if (distance > best)
                    count++;
            }

            product *= count;
        }

        return $"{product}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine).Select(l => l.Split(' ').Skip(1).Where(c => c != string.Empty)
            .Aggregate((a, c) => a + c)).Select(long.Parse).ToArray();

        var product = 1;
        
        var time = lines[0];
        var best = lines[1];

        int count = 0;
        for (int j = 0; j <= time; j++)
        {
            var timeMoving = time - j;
            var distance = j * timeMoving;

            if (distance > best)
                count++;
        }
        
        return $"{count}";
    }
}
