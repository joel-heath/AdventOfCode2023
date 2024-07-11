namespace AdventOfCode2023;
public class Day22 : IDay
{
    public int Day => 22;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9", "5" },
        { "2,3,4~6,6,5\r\n3,4,6~3,4,10\r\n3,4,11~6,6,11\r\n6,6,6~6,6,10", "3" },
        { "2,3,4~6,6,5\r\n3,4,9~3,4,13\r\n3,4,21~6,6,21\r\n6,6,8~6,6,12", "3" },
        { "12,11,4~12,11,7\r\n13,11,4~13,11,7\r\n14,11,4~14,11,7\r\n13,13,4~13,13,7\r\n11,10,8~15,14,8", "5" },
        { "12,11,4~12,11,7\r\n13,11,7~13,11,10\r\n14,11,5~14,11,8\r\n13,13,24~13,13,27\r\n11,10,38~15,14,38", "5" },
        { "12,11,4~12,11,7\r\n13,11,7~13,11,9\r\n14,11,5~14,11,8\r\n13,13,24~13,13,27\r\n11,10,38~15,14,38", "5" },
        { "0,0,0~0,0,1\r\n2,2,0~2,2,2", "2" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9", "7" }
    };

    public string SolvePart1(string input)
        => $"{(new List<(Coord a, Coord b)>[] {
            [..input.Split(Environment.NewLine).SelectMany(l => new List<List<long>>[] { [..l.Split('~').Select(line => line.Split(',').Select(long.Parse).ToList()).OrderBy(i => i[2])] }
                .Select(line => (a:new Coord(line[0][0], line[0][1], line[0][2]), b:new Coord(line[1][0], line[1][1], line[1][2]))))
                .OrderBy(b => b.a.Z).ThenBy(b => b.b.Z)] }.Select(bricks => (bricks, disintegrable:(DropBricks(bricks) == default ? bricks : bricks).ToHashSet()))
            .Select(x => x.bricks.All(stuff => new List<(Coord a, Coord b)>[] { x.bricks.Where(k => k.b.Z == stuff.a.Z - 1 && Crossover(k, stuff)).ToList() }.All(supportingBlocks => supportingBlocks.Count != 1 || x.disintegrable.Remove(supportingBlocks[0]) || true))
                ? x.disintegrable.Count : x.disintegrable.Count)).First()}";

    public string SolvePart2(string input)
        => $"{(new List<(Coord a, Coord b)>[] { [..input.Split(Environment.NewLine).SelectMany(l => new List<List<long>>[] { [..l.Split('~').Select(line => line.Split(',').Select(long.Parse).ToList()).OrderBy(i => i[2])] }
                .Select(line => (a:new Coord(line[0][0], line[0][1], line[0][2]), b:new Coord(line[1][0], line[1][1], line[1][2]))))
                .OrderBy(b => b.a.Z).ThenBy(b => b.b.Z)] }
            .Select(bricks => (bricks: DropBricks(bricks) == default ? bricks : bricks, disintegrable: new HashSet<(Coord a, Coord b)>()))
            .Reverse()
            .Sum(x => (x.bricks.All(stuff => new List<(Coord a, Coord b)>[] { x.bricks.Where(k => k.b.Z == stuff.a.Z - 1 && Crossover(k, stuff)).ToList() }.All(supportingBlocks => supportingBlocks.Count != 1 || x.disintegrable.Add(supportingBlocks[0]) || true))
                ? x.disintegrable : x.disintegrable).Sum(item => DropBricks([.. x.bricks.Where(i => i != item)]))))}";

    private static long DropBricks(List<(Coord a, Coord b)> bricks)
        => bricks.ToList().Select((b, i) => (b, bricks[i] = new long[] { bricks.Where(k => k.b.Z < bricks[i].a.Z && Crossover(bricks[i], k)).Append((a: new Coord(long.MinValue, long.MinValue, 0), b: new Coord(long.MaxValue, long.MaxValue, 0))).Max(k => k.b.Z) + 1 }
            .Select(newZ => (new Coord(bricks[i].a.X, bricks[i].a.Y, newZ), new Coord(bricks[i].b.X, bricks[i].b.Y, bricks[i].b.Z - bricks[i].a.Z + newZ))).First()))
            .Count(t => t.Item2.a.Z < t.b.a.Z);

    private static bool Crossover((Coord a, Coord b) a, (Coord a, Coord b) b)
        => Math.Min(a.a.X, a.b.X) <= Math.Max(b.a.X, b.b.X) && Math.Min(b.a.X, b.b.X) <= Math.Max(a.a.X, a.b.X)
        && Math.Min(a.a.Y, a.b.Y) <= Math.Max(b.a.Y, b.b.Y) && Math.Min(b.a.Y, b.b.Y) <= Math.Max(a.a.Y, a.b.Y);
}