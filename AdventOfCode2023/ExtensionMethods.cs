namespace AdventOfCode2023;
public static class ExtensionMethods
{
    public static T Dump<T>(this T input)
    {
        Console.WriteLine(input);
        return input;
    }

    public static IReadOnlyList<IReadOnlyList<T>> Transpose<T>(this IReadOnlyList<IReadOnlyList<T>> source)
    {
        return Enumerable.Range(0, source[0].Count)
          .Select(c => source
             .Where(line => line != null && line.Count > c)
             .Select(line => line[c])
             .ToArray())
          .ToArray();
    }
}