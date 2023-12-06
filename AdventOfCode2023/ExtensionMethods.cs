namespace AdventOfCode2023;
public static class ExtensionMethods
{
    public static T Dump<T>(this T input)
    {
        Console.WriteLine(input);
        return input;
    }
}