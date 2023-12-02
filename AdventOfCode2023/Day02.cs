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
        { "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", "8" },
        { "TestInput2", "ExpectedOutput2" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green\r\nGame 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue\r\nGame 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red\r\nGame 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red\r\nGame 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", "2286" },
        { "TestInput2", "ExpectedOutput2" }
    };
    public string SolvePart1(string input)
    {
        int totalRed = 12, totalGreen = 13, totalBlue = 14, totalScore = 0;

        var singleSplit = input.Split(Environment.NewLine);
        for (int i = 0; i < singleSplit.Length; i++)
        {
            var success = true;

            var row = singleSplit[i];
            var pattern1 = @"Game \d+: (((\d+) (blue|red|green)(, )?)+(; )?)+";
            IEnumerable<string> stuff;
            try
            {
                stuff = new Regex(pattern1).Matches(row).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures).Skip(1)).First().Select(i => i.Value.TrimEnd(';'));

                var pattern2 = @"(\d+) (red|green|blue)";

                foreach (var a in stuff)
                {
                    var bits = new Regex(pattern2).Matches(a).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures).Skip(1)).Select(i => i.First().Value).ToArray();

                    int red = 0, green = 0, blue = 0;

                    for (int j = 0; j < bits.Length; j += 2)
                    {
                        var count = int.Parse(bits[j]);
                        var col = bits[j + 1];

                        switch (col)
                        {
                            case "red": red += count; break;
                            case "blue": blue += count; break;
                            case "green": green += count; break;
                        }
                    }

                    if (red > totalRed || blue > totalBlue || green > totalGreen)
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    totalScore += i + 1;
                }
            }
            catch
            {

            }


            
        }

        return $"{totalScore}";
    }

    public string SolvePart2(string input)
    {
        int totalScore = 0;
        var singleSplit = input.Split(Environment.NewLine);
        for (int i = 0; i < singleSplit.Length; i++)
        {

            var row = singleSplit[i];
            var pattern1 = @"Game \d+: (((\d+) (blue|red|green)(, )?)+(; )?)+";

            try
            {
                IEnumerable<string> stuff = new Regex(pattern1).Matches(row).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures).Skip(1)).First().Select(i => i.Value.TrimEnd(';'));

                var pattern2 = @"(\d+) (red|green|blue)";

                int minRed = 0, minGreen = 0, minBlue = 0;
                foreach (var a in stuff)
                {
                    var bits = new Regex(pattern2).Matches(a).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures).Skip(1)).Select(i => i.First().Value).ToArray();

                    int red = 0, green = 0, blue = 0;

                    for (int j = 0; j < bits.Length; j += 2)
                    {
                        var count = int.Parse(bits[j]);
                        var col = bits[j + 1];

                        switch (col)
                        {
                            case "red": red += count; break;
                            case "blue": blue += count; break;
                            case "green": green += count; break;
                        }
                    }

                    if (red > minRed) minRed = red;
                    if (green > minGreen) minGreen = green;
                    if (blue > minBlue) minBlue = blue;
                }

                totalScore += minRed * minGreen * minBlue;
            }
            catch
            {

            }
        }

        return $"{totalScore}";

    }
}
