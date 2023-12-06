namespace AdventOfCode2023;
public class Day05 : IDay
{
    public int Day => 5;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4", "35" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4", "46" }
    };

    public string SolvePart1(string input)
        => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }
            .Select(b =>
                new (long[], string[])[] { (b[0].Split(" ")[1..].Select(long.Parse).ToArray(), b[1..]) }
                .Select<(long[] seeds, string[] maps), long>(i =>
                    new IEnumerable<HashSet<(long destination, long source, long range)>>[] { i.maps.Select(m => m.Split(Environment.NewLine)[1..].Select(l => l.Split(" ").Select(long.Parse).ToArray())
                                .Select<long[], (long destination, long source, long range)>(d => (d[0], d[1], d[2])).ToHashSet()) }
                                .Select(mappings =>
                                    i.seeds.Select(s => 
                                        mappings.Aggregate(s, (seedAcc, m) =>
                                            new List<(long destination, long source, long range)> { m.FirstOrDefault(m => m.source <= seedAcc && seedAcc <= m.source + m.range) }
                                            .Select(m => seedAcc - m.source + m.destination).First()))
                                    .Min())
                                .First())
                    .First()).First()}";

    public string SolvePart2(string input)
    => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }
        .Select(b => (b[0].Split(" ")[1..].Select(long.Parse).Chunk(2).Select(i => (i[0], i[1])).ToList(), b[1..]))
        .Select<(List<(long source, long range)> seeds, string[] maps), long>(i =>
            i.maps.Select(m => m.Split(Environment.NewLine)[1..]
                      .Select(l => l.Split(" ").Select(long.Parse).ToArray())
                      .Select<long[], (long destination, long source, long range)>(d => (d[0], d[1], d[2])).ToList())
                  .ToList()
                  .Aggregate(i.seeds, (acc, m) => [.. CalculateNewSeeds(acc, m.OrderBy(s => s.source).ToList())])
                  .Min(x => x.source)).First()}";

    public IEnumerable<(long source, long range)> CalculateNewSeeds(List<(long source, long range)> seeds, IReadOnlyList<(long destination, long source, long range)> mappings)
        => (seeds.Count == 0) ? [] : new List<(long destination, long source, long range)>() { mappings.FirstOrDefault(m => !(seeds[0].source + seeds[0].range <= m.source || seeds[0].source >= m.source + m.range)) }
            .SelectMany(m => m == (0, 0, 0)
                ? new List<(long source, long range)>() { (seeds[0].source, seeds[0].range) }.Concat(CalculateNewSeeds(seeds.Count > 1 ? seeds[1..] : [], mappings)) // [1..]
                : (seeds[0].source < m.source ? new List<(long source, long range)>() { (seeds[0].source, m.source) } : [])
                    .Append((m.destination + Math.Max(seeds[0].source, m.source) - m.source, Math.Min(seeds[0].source + seeds[0].range, m.source + m.range) - Math.Max(seeds[0].source, m.source)))
                    .Concat(CalculateNewSeeds((seeds.Count > 1 ? seeds[1..] : []).Concat(m.source + m.range < seeds[0].source + seeds[0].range ? new List<(long source, long range)>() { (m.source + m.range, seeds[0].range - (m.source + m.range - Math.Max(seeds[0].source, m.source))) } : []).ToList(), mappings)));
}