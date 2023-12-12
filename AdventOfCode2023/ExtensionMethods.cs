namespace AdventOfCode2023;
public static class ExtensionMethods
{
    public static T Dump<T>(this T input)
    {
        Console.WriteLine(input);
        return input;
    }

    public static IEnumerable<(T, long)> RLE<T>(this IEnumerable<T> source)
    {
        var enumerator = source.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;

        var curr = enumerator.Current;
        long count = 1;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current.Equals(curr))
            {
                count++;
            }
            else
            {
                yield return (curr, count);
                count = 1;
                curr = enumerator.Current;
            }
        }
        yield return (curr, count);
    }

    public static T[][] Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        => source.SelectMany(inner => inner.Select((item, index) => new { item, index }))
            .GroupBy(i => i.index, i => i.item)
            .Select(g => g.ToArray()).ToArray();

    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
        => k == 0 ? new[] { Array.Empty<T>() } :
          elements.SelectMany((e, i) =>
            elements.Skip(i + 1).Combinations(k - 1).Select(c => (new[] { e }).Concat(c)));

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

    public static TAccumulate AggregateWhileAvailable<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> accumulator, Func<TAccumulate, TSource, IEnumerable<TSource>> feedback)
    {
        Queue<TSource> queue = new();

        var acc = seed;
        foreach (var item in source)
        {
            acc = accumulator(acc, item);
            foreach (var newItem in feedback(acc, item)) queue.Enqueue(newItem);
        }

        while (queue.TryDequeue(out var item))
        {
            acc = accumulator(acc, item);
            foreach (var newItem in feedback(acc, item)) queue.Enqueue(newItem);
        }

        return acc;
    }
}