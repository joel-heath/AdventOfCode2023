using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public static class Utils
{
    /// <summary>
    /// Enumerable.Range but lazily evaluated & can have negative counts to go in reverse
    /// </summary>
    /// <param name="start">The starting element of the range</param>
    /// <param name="count">The vector difference between the start value and end</param>
    /// <returns>Such numbers as described</returns>
    public static IEnumerable<int> Range(int start, int count)
    {
        int end = start + count;
        int inc = end < start ? -1 : 1;
        while (start != end)
        {
            yield return start;
            start += inc;
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


    public static long GCF(long a, long b)
    {
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a;
    }

    public static long LCM(long a, long b) => a * b / GCF(a, b);
    public static long LCM(params long[] a) => a.Aggregate(LCM);

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
