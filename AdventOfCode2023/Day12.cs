namespace AdventOfCode2023;
public class Day12 : IDay
{
    public int Day => 12;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "?.# 1,1", "1" },
        { ".??.# 1,1", "2" },
        { "?##. 3", "1" },
        { "??##. 1,2", "1" },
        { "???.### 1,1,3", "1" },
        { ".??..??...?##. 1,1,3", "4" },
        { "?#?#?#?#?#?#?#? 1,3,1,6", "1" },
        { "????.#...#... 4,1,1", "1" },
        { "????.######..#####. 1,6,5", "4" },
        { "?###???????? 3,2,1", "10" },
        { "###???????? 3,2,1", "10" },
        { "???.### 1,1,3\r\n.??..??...?##. 1,1,3\r\n?#?#?#?#?#?#?#? 1,3,1,6\r\n????.#...#... 4,1,1\r\n????.######..#####. 1,6,5\r\n?###???????? 3,2,1", "21" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "???.### 1,1,3\r\n.??..??...?##. 1,1,3\r\n?#?#?#?#?#?#?#? 1,3,1,6\r\n????.#...#... 4,1,1\r\n????.######..#####. 1,6,5\r\n?###???????? 3,2,1", "525152" }
    };

    public static long CountArrangements(string line, long[] contiguous, Dictionary<(int, int, long, bool), long> memo, int i = 0, int index = 0, long remaining = -1, bool currentlyHashes = false)
    {
        if (memo.TryGetValue((i, index, remaining, currentlyHashes), out var arrangements)) return arrangements;

        remaining = remaining < 0 ? contiguous[index] : remaining;
        for (; i < line.Length; i++)
        {
            var current = line[i];

            if (current == '#')
            {
                currentlyHashes = true;
                if (remaining == 0) return 0;
                remaining--;
            }
            else if (current == '.')
            {
                if (currentlyHashes && remaining > 0) return 0;
                currentlyHashes = false;
            }
            else // ?
            {
                long options = 0;
                if (remaining > 0) // try #
                    options += CountArrangements(line, contiguous, memo, i + 1, index, remaining - 1, true);

                if (!currentlyHashes || remaining == 0) // try .
                {
                    var newIndex = index;
                    var newRemaining = remaining;
                    if (newRemaining == 0)
                    {
                        newIndex++;
                        if (newIndex < contiguous.Length)
                        {
                            newRemaining = contiguous[newIndex];
                        }
                    }
                    options += CountArrangements(line, contiguous, memo, i + 1, newIndex, newRemaining);
                }
                memo[(i, index, remaining, currentlyHashes)] = options;
                return options;
            }

            if (!currentlyHashes && remaining == 0)
            {
                index++;
                if (index < contiguous.Length)
                {
                    remaining = contiguous[index];
                }
            }
        }

        var answer = index < contiguous.Length ? 0 : 1;
        memo[(i, index, remaining, currentlyHashes)] = answer;
        return answer;
    }

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Sum(l =>
            new string[][] { l.Split(' ') }
                .Sum(d => CountArrangements(d[0] + '.', d[1].Split(',').Select(long.Parse).ToArray(), [])))}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine).Sum(l =>
            new string[][] { l.Split(' ') }
                .Sum(d => CountArrangements(string.Join('?', Enumerable.Repeat(d[0], 5)) + '.',
                    Enumerable.Repeat(d[1].Split(',').Select(long.Parse).ToArray(), 5).SelectMany(n => n).ToArray(), [])))}";
}