namespace AdventOfCode2023;
public class Day03 : IDay
{
    public int Day => 3;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "4361" },
        { "TestInput2", "ExpectedOutput2" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "467..114..\r\n...*......\r\n..35..633.\r\n......#...\r\n617*......\r\n.....+.58.\r\n..592.....\r\n......755.\r\n...$.*....\r\n.664.598..", "467835" },
        { "TestInput2", "ExpectedOutput2" }
    };

    public static IEnumerable<char> GetAllAdjacent(string[] input, int row, int index, int length)
    {
        var left = index - 1;
        if (left > 0)
        {
            yield return input[row][left]; // left
            if (row > 0)
                yield return input[row - 1][left]; // top left
            if (row < input.Length - 1)
                yield return input[row + 1][left]; // bottom left
        }

        var right = index + length;
        if (right < input[row].Length)
        {
            yield return input[row][right]; // right
            if (row > 0)
                yield return input[row - 1][right]; // top left
            if (row < input.Length - 1)
                yield return input[row + 1][right]; // bottom left
        }

        // above and below
        for (int i = 0; i < length; i++)
        {
            if (row > 0)
                yield return input[row - 1][index + i];
            if (row < input.Length - 1)
                yield return input[row + 1][index + i];
        }
    }

    public static IEnumerable<(int, int)> GetAllAdjacentInfo(string[] input, int row, int index, int length)
    {
        var left = index - 1;
        if (left > 0)
        {
            yield return (row, left); // left
            if (row > 0)
                yield return (row - 1, left); // top left
            if (row < input.Length - 1)
                yield return (row + 1, left); // bottom left
        }

        var right = index + length;
        if (right < input[row].Length)
        {
            yield return (row, right); // right
            if (row > 0)
                yield return (row - 1, right); // top left
            if (row < input.Length - 1)
                yield return (row + 1, right); // bottom left
        }

        // above and below
        for (int i = 0; i < length; i++)
        {
            if (row > 0)
                yield return (row - 1, index + i);
            if (row < input.Length - 1)
                yield return (row + 1, index + i);
        }
    }

    public string SolvePart1(string input)
    {
        var lines = input.Split(Environment.NewLine);
        int total = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            var nums = Utils.FindAll(@"\d+", lines[i]).ToArray();

            for (int j = 0; j < nums.Length; j++)
            {
                var num = int.Parse(nums[j].Value);
                var index = nums[j].Index;
                var length = nums[j].Length;

                if (GetAllAdjacent(lines, i, index, length).Any(i => i != '.' && (i < '0' || i > '9')))
                    total += num;
            }
        }


        return $"{total}";
    }

    public static (int, int) FindNum(string row, int index)
    {
        int start = index - 1;
        while (true)
        {
            if (start < 0) break;
            if (row[start] < '0' || row[start] > '9') break;
            start--;
        }

        int end = index + 1;
        while (true)
        {
            if (end >= row.Length) break;
            if (row[end] < '0' || row[end] > '9') break;
            end++;
        }

        Console.WriteLine($"{start}..{end} == {row[(start + 1)..end]}");

        return (start+1,end);
    }

    public string SolvePart2(string input)
    {
        var lines = input.Split(Environment.NewLine);
        int total = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            var nums = Utils.FindAll(@"\*", lines[i]).ToArray();

            for (int j = 0; j < nums.Length; j++)
            {
                //var num = int.Parse(nums[j].Value);
                var index = nums[j].Index;
                var length = nums[j].Length;

                var adjacents = GetAllAdjacentInfo(lines, i, index, length).ToArray();

                var partNums = adjacents.Where(i => lines[i.Item1][i.Item2] >= '0' && lines[i.Item1][i.Item2] <= '9').ToArray();

                var fullNums = partNums.Select(n => (FindNum(lines[n.Item1], n.Item2), n.Item1)).Distinct().ToArray();

                if (fullNums.Length == 2)
                {
                    total += int.Parse(lines[fullNums[0].Item2][fullNums[0].Item1.Item1..fullNums[0].Item1.Item2])
                           * int.Parse(lines[fullNums[1].Item2][fullNums[1].Item1.Item1..fullNums[1].Item1.Item2]);
                }

            }
        }

        return $"{total}";
    }
}
