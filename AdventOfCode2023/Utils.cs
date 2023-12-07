using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace AdventOfCode2023;
public static class Utils
{
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
        => Matches(@"-?\d+", str).Select(long.Parse);

}
