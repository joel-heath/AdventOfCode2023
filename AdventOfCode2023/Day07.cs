namespace AdventOfCode2023;
public class Day07 : IDay
{
    public int Day => 7;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483",
            "6440"
        },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "32T3K 765\r\nT55J5 684\r\nKK677 28\r\nKTJJT 220\r\nQQQJA 483",
            "5905"
        },
    };

    public static int CardValue(char a)
        => a switch
        {
            'A' => 14,
            'K' => 13,
            'Q' => 12,
            'J' => 11,
            'T' => 10,
            _ => int.Parse($"{a}")
        };

    public static int CardValue2(char a)
    => a switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'T' => 10,
        'J' => 1,
        _ => int.Parse($"{a}")
    };


    public static int CompareHands(char[] a, char[] b)
    {
        var aOrdered = a.GroupBy(c => c).Select(g => g.Count()).OrderDescending().ToArray();
        var bOrdered = b.GroupBy(c => c).Select(g => g.Count()).OrderDescending().ToArray();

        var aPair = aOrdered[0];
        var bPair = bOrdered[0];

        if (aPair > bPair) return 1;
        if (bPair > aPair) return -1;

        if (aPair == 2 && aOrdered[1] == 2 && bOrdered[1] == 1) return 1;
        if (bPair == 2 && bOrdered[1] == 2 && aOrdered[1] == 1) return -1;

        if (aPair == 3 && aOrdered[1] == 2 && bOrdered[1] == 1) return 1;
        if (bPair == 3 && bOrdered[1] == 2 && aOrdered[1] == 1) return -1;

        for (var i = 0; i < a.Length; i++)
        {
            var aValue = CardValue(a[i]);
            var bValue = CardValue(b[i]);

            if (aValue > bValue) return 1;
            if (bValue > aValue) return -1;
        }

        return 0;
    }

    static void BubbleSort(List<(char[] cards, long bid)> input)
    {
        for (var i = input.Count; i >= 0; i--)
        {
            for (int j = 0, k = 1; k < i; j++, k++)
            {
                $"Comparing {string.Concat(input[j].cards)} with {string.Concat(input[k].cards)}".Dump();
                var res = CompareHands2(input[j].cards, input[k].cards);

                if (res > 0)
                {
                    (input[j], input[k]) = (input[k], input[j]);
                    "WRONG ORDER".Dump();
                }
            }
        }
    }

    public static int CompareHands2(char[] a, char[] b)
    {
        var aOrdered = a.Where(i => i != 'J').GroupBy(c => c).Select(g => g.Count()).OrderDescending().ToArray();
        var bOrdered = b.Where(i => i != 'J').GroupBy(c => c).Select(g => g.Count()).OrderDescending().ToArray();

        

        var aPair = aOrdered.Length == 0 ? 5 : aOrdered[0] + a.Where(c => c == 'J').Count();
        var bPair = bOrdered.Length == 0 ? 5 : bOrdered[0] + b.Where(c => c == 'J').Count();

        if (aPair > bPair) return 1;
        if (bPair > aPair) return -1;

        if (aPair == 2 && aOrdered[1] == 2 && bOrdered[1] == 1) return 1;
        if (bPair == 2 && bOrdered[1] == 2 && aOrdered[1] == 1) return -1;

        if (aPair == 3 && aOrdered[1] == 2 && bOrdered[1] == 1) return 1;
        if (bPair == 3 && bOrdered[1] == 2 && aOrdered[1] == 1) return -1;

        for (var i = 0; i < a.Length; i++)
        {
            var aValue = CardValue2(a[i]);
            var bValue = CardValue2(b[i]);

            if (aValue > bValue) return 1;
            if (bValue > aValue) return -1;
        }

        return 0;
    }

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var hands = new List<(char[] cards, long bid)>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(" ");

            var cards = line[0].ToCharArray();
            var bid = long.Parse(line[1]);

            hands.Add((cards, bid));
        }

        hands.Sort((a, b) => CompareHands(a.cards, b.cards));

        return $"{hands.Select((c, i) => (i+1) * c.bid).Sum()}";
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        var hands = new List<(char[] cards, long bid)>();

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(" ");

            var cards = line[0].ToCharArray();
            var bid = long.Parse(line[1]);

            hands.Add((cards, bid));
        }

        hands.Sort((a, b) => CompareHands2(a.cards, b.cards));
        //BubbleSort(hands);

        return $"{hands.Select((c, i) => (i + 1) * c.bid).Sum()}";
    }
}
