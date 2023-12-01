using System.Data;

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

    static readonly string[] nums = Enumerable.Range(0, 10).Select(a => $"{a}").Concat(Enumerable.Range(0, 10).Select(i => i switch
    {
        0 => "zero",
        1 => "one",
        2 => "two",
        3 => "three",
        4 => "four",
        5 => "five",
        6 => "six",
        7 => "seven",
        8 => "eight",
        _ => "nine"
    })).ToArray();

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
             _  => 9
    };

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Sum(r => int.Parse($"{r.First(i => i >= '1' && i <= '9')}" + $"{r.Reverse().First(i => i >= '1' && i <= '9')}"))}";

    public string SolvePart2(string input)
        => $"{input.Split(Environment.NewLine).Sum(i =>int.Parse($"{
            StringToNum(nums.Select(n => (i.IndexOf(n), n)).Select(e => e.Item1 < 0 ? (int.MaxValue, e.n) : e).MinBy(e => e.Item1).n)}" + $"{
            StringToNum(nums.Select(n => (string.Concat(i.Reverse()).IndexOf(string.Concat(n.Reverse())), n)).Select(e => e.Item1 < 0 ? (int.MaxValue, e.n) : e).MinBy(e => e.Item1).n)}"))}";
}
