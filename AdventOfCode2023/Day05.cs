using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AdventOfCode2023;
public class Day05 : IDay
{
    public int Day => 5;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4",
            "35"
        }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "seeds: 79 14 55 13\r\n\r\nseed-to-soil map:\r\n50 98 2\r\n52 50 48\r\n\r\nsoil-to-fertilizer map:\r\n0 15 37\r\n37 52 2\r\n39 0 15\r\n\r\nfertilizer-to-water map:\r\n49 53 8\r\n0 11 42\r\n42 0 7\r\n57 7 4\r\n\r\nwater-to-light map:\r\n88 18 7\r\n18 25 70\r\n\r\nlight-to-temperature map:\r\n45 77 23\r\n81 45 19\r\n68 64 13\r\n\r\ntemperature-to-humidity map:\r\n0 69 1\r\n1 0 69\r\n\r\nhumidity-to-location map:\r\n60 56 37\r\n56 93 4",
            "46"
        },
    };

    public static long Map(HashSet<(long destination, long source, long range)> mappings, long num)
    {
        foreach (var (destination, source, range) in mappings)
        {
            if (source <= num && num <= source + range)
            {
                return num - source + destination;
            }
        }

        return num;
    }

    public string SolvePart1(string input)
        => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }
            .Select(b =>
                new (long[], string[])[] { (b[0].Split(" ")[1..].Select(long.Parse).ToArray(), b[1..]) }
                .Select<(long[] seeds, string[] maps), long>(i =>
                    new IEnumerable<HashSet<(long destination, long source, long range)>>[] { i.maps.Select(m => m.Split(Environment.NewLine)[1..].Select(l => l.Split(" ").Select(long.Parse).ToArray())
                                .Select<long[], (long destination, long source, long range)>(d => (d[0], d[1], d[2])).ToHashSet()) }
                                .Select(mappings =>
                                    i.seeds.Select(s => 
                                        mappings.Aggregate(s, (seedAcc, m) => Map(m, seedAcc))
                                    ).Min()
                                ).First()
                    ).First()).First()}";

    public IEnumerable<(long source, long range)> MapSeedRanges(List<(long source, long range)> seeds, IReadOnlyList<(long destination, long source, long range)> mappings)
    {
        if (seeds.Count == 0) return [];
        var s = seeds[0];

        return new (long destination, long source, long range)[] { mappings.FirstOrDefault(m => s.source + s.range <= m.source || s.source >= m.source + m.range) }
                .SelectMany(m => m == (0, 0, 0)
                ? new List<(long source, long range)>() { (s.source, s.range) }.Concat(MapSeedRanges(seeds.Count > 1 ? seeds[1..] : [], mappings))
                : (s.source < m.source ? [(s.source, m.source)] : new List<(long source, long range)>())
                            .Append((m.destination + Math.Max(s.source, m.source) - m.source, Math.Min(s.source + s.range, m.source + m.range) - Math.Max(s.source, m.source)))
                            //.ToList()
                            .Concat(MapSeedRanges([..(seeds.Count > 1 ? seeds[1..] : []).Concat(m.source + m.range < s.source + s.range ? [(m.source + m.range, s.range - (m.source + m.range - Math.Max(s.source, m.source)))] : Array.Empty<(long source, long range)>())], mappings))
                            //.ToList()
                    );
    }

    public string SolvePart2WorkInProgressOneLiner(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);
        var seeds = blocks[0].Split(" ")[1..].Select(long.Parse).Chunk(2).Select(i => (i[0], i[1])).ToList();
        var maps = blocks[1..];

        for (long i = 0; i < maps.Length; i++)
        {
            var mappings = new IEnumerable<List<(long destination, long source, long range)>>[] { maps.Select(m => m.Split(Environment.NewLine)[1..].Select(l => l.Split(" ").Select(long.Parse).ToArray())
                                .Select<long[], (long destination, long source, long range)>(d => (d[0], d[1], d[2])).ToList()) }
                                .First().First();

            seeds = [.. seeds.OrderBy(s => s.Item1)];
            mappings = [.. mappings.OrderBy(s => s.source)];

            List<(long source, long range)> newSeeds = [];
            //var queue = new Queue<(long source, long range)>(seeds);

            seeds = MapSeedRanges(seeds, mappings).ToList();
        }

        return $"{seeds.Min(x => x.Item1)}";
    }

    public string SolvePart2(string input)
    {
        var blocks = input.Split(Environment.NewLine + Environment.NewLine);
        var seeds = blocks[0].Split(" ")[1..].Select(long.Parse).Chunk(2).Select(i => (i[0], i[1])).ToList();
        var maps = blocks[1..];

        for (long i = 0; i < maps.Length; i++)
        {
            var lines = maps[i].Split(Environment.NewLine)[1..];

            var mappings = new List<(long destination, long source, long range)>();

            for (long j = 0; j < lines.Length; j++)
            {
                var line = lines[j];

                var data = line.Split(" ").Select(long.Parse).ToArray();

                var destination = data[0];
                var source = data[1];
                var range = data[2];


                mappings.Add((destination, source, range));
            }

            seeds = [.. seeds.OrderBy(s => s.Item1)];
            mappings = [.. mappings.OrderBy(s => s.source)];

            List<(long source, long range)> newSeeds = [];
            var queue = new Queue<(long source, long range)>(seeds);

            while (queue.TryDequeue(out var seed))
            {
                bool mappedSeed = false;
                foreach (var mapping in mappings)
                {
                    if (seed.source + seed.range <= mapping.source || seed.source >= mapping.source + mapping.range) continue;

                    long start = seed.source;
                    if (seed.source < mapping.source)
                    {
                        newSeeds.Add((seed.source, mapping.source));
                        start = mapping.source;
                    }

                    long seedEnd = seed.source + seed.range;
                    long mappingEnd = mapping.source + mapping.range;

                    long end = seedEnd;

                    if (mappingEnd < seedEnd)
                    {
                        queue.Enqueue((mappingEnd, seed.range - (mappingEnd - start)));
                        end = mappingEnd;
                    }

                    long index = start - mapping.source;
                    long range = end - start;

                    newSeeds.Add((mapping.destination + index, range));

                    mappedSeed = true;
                    break;
                }

                if (!mappedSeed)
                    newSeeds.Add((seed.source, seed.range));
            }

            seeds = [.. newSeeds];
        }

        return $"{seeds.Min(x => x.Item1)}";
    }
}