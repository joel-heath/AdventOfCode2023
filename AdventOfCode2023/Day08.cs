using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public class Day08 : IDay
{
    public int Day => 8;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "RL\r\n\r\nAAA = (BBB, CCC)\r\nBBB = (DDD, EEE)\r\nCCC = (ZZZ, GGG)\r\nDDD = (DDD, DDD)\r\nEEE = (EEE, EEE)\r\nGGG = (GGG, GGG)\r\nZZZ = (ZZZ, ZZZ)",
            "2"
        },
        {
            "LLR\r\n\r\nAAA = (BBB, BBB)\r\nBBB = (AAA, ZZZ)\r\nZZZ = (ZZZ, ZZZ)",
            "6"
        }

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "LR\r\n\r\n11A = (11B, XXX)\r\n11B = (XXX, 11Z)\r\n11Z = (11B, XXX)\r\n22A = (22B, XXX)\r\n22B = (22C, 22C)\r\n22C = (22Z, 22Z)\r\n22Z = (22B, 22B)\r\nXXX = (XXX, XXX)",
            "6"
        },

    };

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine + Environment.NewLine);

        var instructions = lines[0];

        var graph = new Regex(@"([A-Z])([A-Z])([A-Z])").Matches(lines[1]).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0].Value))
                    .Where((c, i) => i % 4 == 0)
                    .Chunk(3)
                    .ToDictionary(i => i[0], i => (i[1], i[2]));

        var current = "AAA";

        long j = 0;
        for (; current != "ZZZ"; j++)
        {
            (string left, string right) = graph[current];

            current = instructions[(int)(j % instructions.Length)] == 'L'
                    ? left
                    : right;
        }

        return $"{j}";
    }

    static long GCF(long a, long b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }

    static long LCM(long a, long b)
        => a * b / GCF(a, b);

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine + Environment.NewLine);

        var instructions = lines[0];

        var graph = new Regex(@"([A-Z0-9])([A-Z0-9])([A-Z0-9])").Matches(lines[1]).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0].Value))
                    .Where((c, i) => i % 4 == 0)
                    .Chunk(3)
                    .ToDictionary(i => i[0], i => (i[1], i[2]));

        var currents = graph.Keys.Where(k => k[2] == 'A').ToArray();
        var lengths = new long[currents.Length];

        for (int i = 0; i < currents.Length; i++)
        {
            var current = currents[i];
            long j = 0;
            for (; current[2] != 'Z'; j++)
            {
                (string left, string right) = graph[current];

                current = instructions[(int)(j % instructions.Length)] == 'L'
                        ? left
                        : right;
            }

            lengths[i] = j;
        }

        return $"{lengths.Aggregate(LCM)}";
    }
}