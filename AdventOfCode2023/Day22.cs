namespace AdventOfCode2023;
public class Day22 : IDay
{
    public int Day => 22;
    public Dictionary<string, string> UnitTestsP1 => new()
    {

        {
            "1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9",
            "5"
        },
        {
            "2,3,4~6,6,5\r\n3,4,6~3,4,10\r\n3,4,11~6,6,11\r\n6,6,6~6,6,10",
            "3"
        },
        {
            "2,3,4~6,6,5\r\n3,4,9~3,4,13\r\n3,4,21~6,6,21\r\n6,6,8~6,6,12",
            "3"
        },
        {
            "12,11,4~12,11,7\r\n13,11,4~13,11,7\r\n14,11,4~14,11,7\r\n13,13,4~13,13,7\r\n11,10,8~15,14,8",
            "5"
        },
        {
            "12,11,4~12,11,7\r\n13,11,7~13,11,10\r\n14,11,5~14,11,8\r\n13,13,24~13,13,27\r\n11,10,38~15,14,38",
            "5"
        },
        {
            "12,11,4~12,11,7\r\n13,11,7~13,11,9\r\n14,11,5~14,11,8\r\n13,13,24~13,13,27\r\n11,10,38~15,14,38",
            "5"
        },
        {
            "0,0,0~0,0,1\r\n2,2,0~2,2,2",
            "2"
        },
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "1,0,1~1,2,1\r\n0,0,2~2,0,2\r\n0,2,3~2,2,3\r\n0,0,4~0,2,4\r\n2,0,5~2,2,5\r\n0,1,6~2,1,6\r\n1,1,8~1,1,9",
            "7"
        }
    };

    public string SolvePart1(string input)
    {
        List<(Coord a, Coord b)> bricks = [];
        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split('~').Select(line => line.Split(',').Select(long.Parse).ToList())
                .OrderBy(i => i[2]).ThenBy(i => i[0]).ThenBy(i => i[1]).ToList();
            var a = line[0];
            var b = line[1];

            bricks.Add((new Coord(a[0], a[1], a[2]), new Coord(b[0], b[1], b[2])));
        }

        bricks = [.. bricks.OrderBy(b => b.a.Z).ThenBy(b => b.b.Z)];

        DropBricks(bricks);

        //HashSet<(Coord a, Coord b)> disintegrable = [];

        HashSet<(Coord a, Coord b)> disintegrable = bricks.ToHashSet();

        foreach (var (a, b) in bricks)
        {
            var supportingBlocks = bricks.Where(k => k.b.Z == a.Z - 1 && Crossover2D(k, (a, b))).ToList();

            if (supportingBlocks.Count == 1)
            {
                disintegrable.Remove(supportingBlocks[0]);
            }
            /*
            if (!bricks.Where(k => k.a.Z == b.Z + 1).Any(k => Crossover2D(k, (a, b))))
            {
                disintegrable.Add((a, b));
            }

            var supportingBlocks = bricks.Where(k => k.b.Z == a.Z - 1 && Crossover2D(k, (a, b))).ToList();

            if (supportingBlocks.Count > 1)
            {
                foreach (var block in supportingBlocks)
                    disintegrable.Add(block);
            }
            */
        }

        return $"{disintegrable.Count}";
    }

    private static void DropBricks(List<(Coord a, Coord b)> bricks)
    {
        var floor = (a:new Coord(long.MinValue, long.MinValue, 0), b:new Coord(long.MaxValue, long.MaxValue, 0));
        for (int i = 0; i < bricks.Count; i++)
        {
            var (a, b) = bricks[i];
            var newZ = bricks.Where(k => k.b.Z < a.Z && Crossover2D((a, b), k)).Append(floor).Max(k => k.b.Z) + 1;
            bricks[i] = (new Coord(a.X, a.Y, newZ), new Coord(b.X, b.Y, b.Z - a.Z + newZ));
        }
    }

    private static bool Crossover2D((Coord a, Coord b) a, (Coord a, Coord b) b)  => Comparison2D(a, b) || Comparison2D(b, a);

    private static bool Comparison2D((Coord a, Coord b) a, (Coord a, Coord b) b)
        => Math.Min(a.a.X, a.b.X) <= Math.Max(b.a.X, b.b.X) && Math.Min(b.a.X, b.b.X) <= Math.Max(a.a.X, a.b.X)
        && Math.Min(a.a.Y, a.b.Y) <= Math.Max(b.a.Y, b.b.Y) && Math.Min(b.a.Y, b.b.Y) <= Math.Max(a.a.Y, a.b.Y);
        

    public string SolvePart2(string input)
    {
        List<(Coord a, Coord b)> bricks = [];
        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split('~').Select(line => line.Split(',').Select(long.Parse).ToList())
                .OrderBy(i => i[2]).ThenBy(i => i[0]).ThenBy(i => i[1]).ToList();
            var a = line[0];
            var b = line[1];

            bricks.Add((new Coord(a[0], a[1], a[2]), new Coord(b[0], b[1], b[2])));
        }

        bricks = [.. bricks.OrderBy(b => b.a.Z).ThenBy(b => b.b.Z)];

        DropBricks(bricks);

        HashSet<(Coord a, Coord b)> toDisintegrate = [];

        foreach (var (a, b) in bricks)
        {
            var supportingBlocks = bricks.Where(k => k.b.Z == a.Z - 1 && Crossover2D(k, (a, b))).ToList();

            if (supportingBlocks.Count == 1)
            {
                toDisintegrate.Add(supportingBlocks[0]);
            }
        }

        long total = 0;
        foreach (var item in toDisintegrate)
        {
            total += CountFalls([.. bricks.Where(i => i != item)]);
        }

        return $"{total}";
    }

    private static long CountFalls(List<(Coord a, Coord b)> bricks)
    {
        var floor = (a: new Coord(long.MinValue, long.MinValue, 0), b: new Coord(long.MaxValue, long.MaxValue, 0));
        long count = 0;
        for (int i = 0; i < bricks.Count; i++)
        {
            var (a, b) = bricks[i];
            var newZ = bricks.Where(k => k.b.Z < a.Z && Crossover2D((a, b), k)).Append(floor).Max(k => k.b.Z) + 1;
            if (newZ < a.Z) count++;
            bricks[i] = (new Coord(a.X, a.Y, newZ), new Coord(b.X, b.Y, b.Z - a.Z + newZ));
        }

        return count;
    }
}