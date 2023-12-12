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

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Sum(l =>
            new string[][] { l.Split(' ') }
                .Sum(d => CountArrangements(d[0] + '.', d[1].Split(',').Select(long.Parse).ToArray(), [])))}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine).Sum(l =>
            new string[][] { l.Split(' ') }
                .Sum(d => CountArrangements(string.Join('?', Enumerable.Repeat(d[0], 5)) + '.',
                    Enumerable.Repeat(d[1].Split(',').Select(long.Parse).ToArray(), 5).SelectMany(n => n).ToArray(), [])))}";

    public static long CountArrangements(string line, long[] contiguous, Dictionary<(int, int, long, bool), long> memo, int i = 0, int index = 0, long remaining = -1, bool currentlyHashes = false)
    {
        if (memo.TryGetValue((i, index, remaining, currentlyHashes), out var arrangements)) return arrangements;
        remaining = remaining < 0 ? contiguous[index] : remaining;

        for (; i < line.Length; i++)
        {
            var current = line[i];

            if (current == '#')
            {
                if (remaining-- == 0 == (currentlyHashes = true)) return memo[(i, index, remaining, currentlyHashes)] = 0;
            }
            else if (current == '.')
            {
                if (currentlyHashes && remaining > 0 || (currentlyHashes = false)) return memo[(i, index, remaining, currentlyHashes)] = 0;
            }
            else
            {
                return new long[] { (remaining > 0
                            ? CountArrangements(line, contiguous, memo, i + 1, index, remaining - 1, true)
                            : 0)
                        + (!currentlyHashes || remaining == 0
                            ? (CountArrangements(line, contiguous, memo, i + 1,
                                remaining == 0
                                    ? index + 1
                                    : index,
                                remaining == 0 && index + 1 < contiguous.Length
                                    ? contiguous[index + 1]
                                    : remaining))
                            : 0)}
                    .Sum(options => memo[(i, index, remaining, currentlyHashes)] = options);
            }

            if (!currentlyHashes && remaining == 0 && (++index < contiguous.Length)) remaining = contiguous[index];
        }

        return memo[(i, index, remaining, currentlyHashes)] = index < contiguous.Length ? 0 : 1;
    }

    public static long OneLiner(string line, long[] contiguous, Dictionary<(int, int, long, bool), long> memo, int i = 0, int index = 0, long remaining = -1, bool currentlyHashes = false)
        => memo.TryGetValue((i, index, remaining = remaining < 0 ? contiguous[index] : remaining, currentlyHashes), out var arrangements) ? arrangements
            : new long[] {
            line.Select((c, i) => (c, i)).Skip(i).AggregateWhile(-1L, (acc, current) =>
            new long[] {
                current.c == '#'
                ? (remaining-- == 0 == (currentlyHashes = true)) ? memo[(current.i, index, remaining, currentlyHashes)] = 0 : acc
                : (current.c == '.')
                    ? (currentlyHashes && remaining > 0 || (currentlyHashes = false)) ? memo[(current.i, index, remaining, currentlyHashes)] = 0 : acc
                    : new long[] { (remaining > 0
                            ? OneLiner(line, contiguous, memo, current.i + 1, index, remaining - 1, true)
                            : 0)
                        + (!currentlyHashes || remaining == 0
                            ? OneLiner(line, contiguous, memo, current.i + 1,
                                remaining == 0
                                    ? index + 1
                                    : index,
                                remaining == 0 && index + 1 < contiguous.Length
                                    ? contiguous[index + 1]
                                    : remaining)
                            : 0)}
                    .Sum(options => memo[(current.i, index, remaining, currentlyHashes)] = options) }
            .Sum(newAcc => (
                (!currentlyHashes && remaining == 0 && (++index < contiguous.Length)) ? remaining = contiguous[index] : remaining)
                    == remaining ? newAcc : newAcc), acc => acc == -1) }
            .Sum(ans => ans != -1 ? ans : memo[(i, index, remaining, currentlyHashes)] = index < contiguous.Length ? 0 : 1);
}