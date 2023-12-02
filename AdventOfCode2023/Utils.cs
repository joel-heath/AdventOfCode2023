using System.Text.RegularExpressions;

namespace AdventOfCode2023;
public static class Utils
{
    public static bool Match(string pattern, string str)
        => new Regex(pattern).Match(str).Success;

    public static IEnumerable<Capture> FindAll(string pattern, string str)
        => new Regex(pattern).Matches(str).SelectMany(m => m.Groups.Cast<Group>().Select(g => g.Captures[0]));

    public static IEnumerable<Capture> FindAllOverlap(string pattern, string str)
        => new Regex("(?=(" + pattern + "))").Matches(str).SelectMany(m => m.Groups[1].Captures);
}
