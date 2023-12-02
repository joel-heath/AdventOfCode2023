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

    public string SolvePart1(string input)
        => $"{input.Split(Environment.NewLine).Sum(r => int.Parse($"{r.First(i => i >= '1' && i <= '9')}" + $"{r.Last(i => i >= '1' && i <= '9')}"))}";

    public string SolvePart2(string input)
        => SolvePart1(input.Replace("zero", "zero0zero").Replace("one", "one1one").Replace("two", "two2two").Replace("three", "three3three").Replace("four", "four4four").Replace("five", "five5five").Replace("six", "six6six").Replace("seven", "seven7seven").Replace("eight", "eight8eight").Replace("nine", "nine9nine"));
}