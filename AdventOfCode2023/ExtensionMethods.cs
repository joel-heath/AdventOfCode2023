namespace AdventOfCode2023;
public static class ExtensionMethods
{
    public static T Dump<T>(this T input)
    {
        Console.WriteLine(input);
        return input;
    }

    public static IEnumerable<TAcc> Scan<TSource, TAcc>(this IEnumerable<TSource> source, TAcc seed, Func<TAcc, TSource, TAcc> func)
    {
        var acc = seed;
        foreach (var item in source)
        {
            acc = func(acc, item);
            yield return acc;
        }
    }

    public static IEnumerable<TSource> Scan<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, TSource> func)
    {
        var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
        TSource acc = enumerator.Current;
        acc = func(acc, enumerator.Current);
        yield return acc;

        while (enumerator.MoveNext())
        {
            acc = func(acc, enumerator.Current);
            yield return acc;
        }
    }
}