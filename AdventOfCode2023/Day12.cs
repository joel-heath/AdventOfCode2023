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
        {
            "TestInput1",
            "ExpectedOutput1"
        },

    };

    public static long CountArrangements(string line, long[] contiguous, int i = 0, int index = 0, long remaining = -1, bool currentlyHashes = false)
    {
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
                    options += CountArrangements(line, contiguous, i + 1, index, remaining - 1, true);

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
                    options += CountArrangements(line, contiguous, i + 1, newIndex, newRemaining);
                }
                

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

        if (index < contiguous.Length) return 0;

        return 1;
    }


    public string SolvePart1(string input)
    {
        long total = 0;

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var data = lines[i].Split(' ');
            
            var goal = data[1].Split(',').Select(long.Parse).ToArray();
            var line = data[0] + '.';

            //var contiguous = line[1].Split(',').Select(long.Parse).ToArray();
            //var strings = line[0].Split('.').Where(l => l != string.Empty).Select(s => s.RLE().ToArray()).ToArray();

            total += CountArrangements(line, goal);
        }


        return $"{total}";
    }

    public string SolvePart2(string input)
    {



        return string.Empty;
    }
}