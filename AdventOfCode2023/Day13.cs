namespace AdventOfCode2023;
public class Day13 : IDay
{
    public int Day => 13;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#",
            "405"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "#.##..##.\r\n..#.##.#.\r\n##......#\r\n##......#\r\n..#.##.#.\r\n..##..##.\r\n#.#.##.#.\r\n\r\n#...##..#\r\n#....#..#\r\n..##..###\r\n#####.##.\r\n#####.##.\r\n..##..###\r\n#....#..#",
            "400"
        },

    };

    static bool IsLineOfReflection(char[][] input, int index)
    {
        for (int i = index, j = index + 1; i >= 0 && j < input.Length; i--, j++)
        {
            for (int k = 0; k < input[i].Length; k++)
            {
                if (input[i][k] != input[j][k]) return false;
            }
        }
        return true;
    }

    static long GetPatternScore(string pattern)
    {
        var lines = pattern.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (IsLineOfReflection(lines, i))
            {
                return 100 * (i + 1);
            }
        }
        lines = lines.Transpose().Select(l => l.ToArray()).ToArray();
        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (IsLineOfReflection(lines, i))
            {
                return i+1;
            }
        }

        throw new Exception();
    }

    static bool IsLineOfReflection2(char[][] input, int index)
    {
        bool haveFoundSmudge = false;
        for (int i = index, j = index + 1; i >= 0 && j < input.Length; i--, j++)
        {
            for (int k = 0; k < input[i].Length; k++)
            {
                if (input[i][k] != input[j][k])
                {
                    if (haveFoundSmudge)
                        return false;
                    else
                        haveFoundSmudge = true;
                }
            }
        }
        if (!haveFoundSmudge) return false;

        return true;
    }

    static long GetPatternScore2(string pattern)
    {
        var lines = pattern.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (IsLineOfReflection2(lines, i))
            {
                return 100 * (i + 1);
            }
        }
        lines = lines.Transpose().Select(l => l.ToArray()).ToArray();
        for (int i = 0; i < lines.Length - 1; i++)
        {
            if (IsLineOfReflection2(lines, i))
            {
                return i + 1;
            }
        }

        throw new Exception();
    }

    public string SolvePart1(string input)
    {
        long total = 0;

        var patterns = input.Split(Environment.NewLine + Environment.NewLine);
        for (int i = 0; i < patterns.Length; i++)
        {
            total += GetPatternScore(patterns[i]);
        }

        return $"{total}";
    }

    public string SolvePart2(string input)
    {
        long total = 0;

        var patterns = input.Split(Environment.NewLine + Environment.NewLine);
        for (int i = 0; i < patterns.Length; i++)
        {
            total += GetPatternScore2(patterns[i]);
        }

        return $"{total}";
    }
}