﻿using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public static class Utils
{
    /// <summary>
    /// Standard modulo doesn't exhibit expected behavour for looping and wrapping indices (-1 % 5 != 4). This modulo function however works this way.
    /// </summary>
    /// <param name="dividend">a in a % b (dividend)</param>
    /// <param name="divisor">b in a % b (modolus)</param>
    /// <returns>The reaminder of the dividend and the divisor</returns>
    public static long Mod(long dividend, long divisor)
    {
        long r = dividend % divisor;
        return r < 0 ? r + divisor : r;
    }

    /// <summary>
    /// Enumerable.Range but lazily evaluated & can have negative counts to go in reverse
    /// </summary>
    /// <param name="start">The starting element of the range</param>
    /// <param name="count">The vector difference between the start value and end</param>
    /// <returns>Such numbers as described</returns>
    public static IEnumerable<long> Range(long start, long count)
    {
        long end = start + count;
        long inc = end < start ? -1 : 1;
        while (start != end)
        {
            yield return start;
            start += inc;
        }
    }

    /// <summary>
    /// Utils.Range but with a custom increment, therefore count is absolute
    /// </summary>
    /// <param name="start">The starting element of the range</param>
    /// <param name="count">The vector difference between the start value and end</param>
    /// <returns>Such numbers as described</returns>
    public static IEnumerable<long> Range(long start, long count, long step)
    {
        long end = start + count * step;
        while (start != end)
        {
            yield return start;
            start += step;
        }
    }

    /// <summary>
    /// Infinitely yields the value parameter (intended to be used with AggregateWhile)
    /// </summary>
    /// <typeparam name="T">Type of value</typeparam>
    /// <param name="value">Value to be server on each enumeration</param>
    /// <returns>Infinite copies of value</returns>
    public static IEnumerable<T> EnumerateForever<T>(T value)
    {
        while (true) yield return value;
    }
    public static IEnumerable<object> EnumerateForever()
    {
        var obj = new object();
        while (true) yield return obj;
    }

    public static long GCF(long a, long b) => EnumerateForever().AggregateWhile((a, b), (acc, _) => (acc.b, acc.a % acc.b), acc => acc.b != 0).a;
    public static long LCM(long a, long b) => a * b / GCF(a, b);
    public static long LCM(params long[] a) => a.Aggregate(LCM);
    public static long LCM(IEnumerable<long> a) => a.Aggregate(LCM);

    public static (List<(string name, Dictionary<int, T> edges)> graph, Dictionary<string, int> nameToIndexMap) GenerateGraph<T>(IEnumerable<(string source, string destination, T weight)> mappings, bool directed = false)
    {
        Dictionary<string, int> nameToIndex = [];
        List<(string name, Dictionary<int, T> edges)> graph = [];

        foreach ((string source, string destination, T weight) in mappings)
        {
            if (!nameToIndex.TryGetValue(source, out int fromIndex))
            {
                fromIndex = graph.Count;
                graph.Add((source, []));
                nameToIndex[source] = fromIndex;
            }
            if (!nameToIndex.TryGetValue(destination, out int toIndex))
            {
                toIndex = graph.Count;
                graph.Add((destination, []));
                nameToIndex[destination] = toIndex;
            }

            var fromEdges = graph[fromIndex].edges;
            fromEdges[toIndex] = weight;

            if (!directed)
            {
                var toEdges = graph[toIndex].edges;
                toEdges[fromIndex] = weight;
            }
        }

        return (graph, nameToIndex);
    }

    public static bool IsMatch(string pattern, string str)
    => new Regex(pattern).Match(str).Success;

    public static Dictionary<string, string> MatchNamed(string pattern, string str)
        => new Regex(pattern).Match(str).Groups.Cast<Group>().ToDictionary(t => t.Name, t => t.Value);

    public static string[] Matches(string pattern, string str)
        => new Regex(pattern).Match(str).Groups.Cast<Group>().Select(t => t.Value).ToArray();

    public static IEnumerable<Capture> FindAll(string pattern, string str)
        => new Regex(pattern).Matches(str).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0]));

    public static IEnumerable<Capture> FindAllOverlap(string pattern, string str)
        => new Regex("(?=(" + pattern + "))").Matches(str).SelectMany(m => m.Groups[1].Captures);

    public static IEnumerable<long> GetLongs(string str)
        => FindAll(@"-?\d+", str).Select(M => long.Parse(M.Value));

}
