namespace AdventOfCode2023;
public class Day09 : IDay
{
    public int Day => 9;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45",
            "114"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "0 3 6 9 12 15\r\n1 3 6 10 15 21\r\n10 13 16 21 30 45",
            "2"
        },

    };

    public string SolvePart1(string input)
    {
        long total = 0;

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var nums = Utils.GetLongs(line).ToArray();

            List<long[]> numThings = [nums];
            bool done = false;


            while (!done)
            {
                long[] newArray = new long[numThings[^1].Length - 1];
                for (int j = 0, k = 1; k < numThings[^1].Length; j++, k++)
                {
                    newArray[j] = numThings[^1][k] - numThings[^1][j];
                }
                numThings.Add(newArray);
                if (newArray.All(x => x == 0))
                    done = true;
            }

            long newValue = 0;
            numThings.Reverse();
            for (int j = 1; j < numThings.Count; j++)
            {
                newValue += numThings[j][^1];
            }

            total += newValue;
        }


        return $"{total}";
    }


    public string SolvePart2(string input)
    {
        long total = 0;

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];

            var nums = Utils.GetLongs(line).ToArray();

            List<long[]> numThings = [nums];
            bool done = false;


            while (!done)
            {
                long[] newArray = new long[numThings[^1].Length - 1];
                for (int j = 0, k = 1; k < numThings[^1].Length; j++, k++)
                {
                    newArray[j] = numThings[^1][k] - numThings[^1][j];
                }
                numThings.Add(newArray);
                if (newArray.All(x => x == 0))
                    done = true;
            }

            long newValue = 0;
            numThings.Reverse();
            for (int j = 1; j < numThings.Count; j++)
            {
                newValue = numThings[j][0] - newValue;
            }

            total += newValue;
        }


        return $"{total}";
    }
}