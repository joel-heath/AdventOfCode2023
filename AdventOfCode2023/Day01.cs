namespace AdventOfCode2023;
public class Day01 : IDay
{
    public int Day => 1;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "1abc2", "12" },
        { "pqr3stu8vwx", "38" },
        { "a1b2c3d4e5f", "15" },
        { "treb7uchet", "77" },
        { "1abc2\r\npqr3stu8vwx\r\na1b2c3d4e5f\r\ntreb7uchet", "142" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "two1nine", "29" },
        { "eightwothree", "83" },
        { "abcone2threexyz", "13" },
        { "xtwone3four", "24" },
        { "4nineeightseven2", "42" },
        { "zoneight234", "14" },
        { "7pqrstsixteen", "76" },
        { "two1nine\r\neightwothree\r\nabcone2threexyz\r\nxtwone3four\r\n4nineeightseven2\r\nzoneight234\r\n7pqrstsixteen", "281" }
    };

    static string[] Split(string input, string? delimeter = null)
    {
        delimeter ??= Environment.NewLine;
        return input.Split(delimeter);
    }

    static string NumToString(int input)
        => string.Concat(input.ToString().Select(i => i switch {
            '0' => "zero",
            '1' => "one",
            '2' => "two",
            '3' => "three",
            '4' => "four",
            '5' => "five",
            '6' => "six",
            '7' => "seven",
            '8' => "eight",
            '9' => "nine"
    }));

    static int StringToNum(string input)
        => input switch {
            "zero" => 0,
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            "0" => 0,
            "1" => 1,
            "2" => 2,
            "3" => 3,
            "4" => 4,
            "5" => 5,
            "6" => 6,
            "7" => 7,
            "8" => 8,
            "9" => 9,
    };


    public string SolvePart1(string input)
    {
        int output = 0;

        string[] singleSplit = Split(input);


        for (int i = 0; i < singleSplit.Length; i++)
        {
            string row = singleSplit[i];
            var a = row.First(i => i >= '1' && i <= '9');
            var b = row.Reverse().First(i => i >= '1' && i <= '9');

            output += int.Parse($"{a}" + $"{b}");
        }



        return $"{output}";
    }

    public string SolvePart2(string input)
    {
        var nums = Enumerable.Range(0,10).Select(a => $"{a}").Concat(Enumerable.Range(0, 10).Select(NumToString)).ToArray();
        var numsBack = nums.Select(i => string.Concat(i.Reverse())).ToArray();

        int output = 0;

        string[] singleSplit = Split(input);


        for (int i = 0; i < singleSplit.Length; i++)
        {
            string item = singleSplit[i];
            string item2 = string.Concat(item.Reverse());

            var indicesMin = new (int i, string j)[nums.Length];
            var indicesMax = new(int i, string j)[nums.Length];

            for (int k = 0; k < nums.Length; k++)
            {
                string number = nums[k];
                string numBack = numsBack[k];
                indicesMin[k] = (item.IndexOf(number), number);
                indicesMax[k] = (item2.IndexOf(numBack), number);
            }

            var a = indicesMin.Select(e => e.i < 0 ? (int.MaxValue, e.j) : e).MinBy(e => e.Item1).j;
            var b = indicesMax.Select(e => e.i < 0 ? (int.MaxValue, e.j) : e).MinBy(e => e.Item1).j;

            output += int.Parse($"{StringToNum(a)}" + $"{StringToNum(b)}");
        }

        return $"{output}";
    }
}
