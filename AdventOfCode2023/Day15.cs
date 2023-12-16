namespace AdventOfCode2023;
public class Day15 : IDay
{
    public int Day => 15;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", "1320" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7", "145" }
    };

    public string SolvePart1(string input)
        => $"{input.Split(',').Sum(l => l.Aggregate(0, (hash, c) => (hash + c) * 17 % 256))}";

    public string SolvePart2(string input)
        => $"{input.Split(',').Select(l => l.Split('=')).Select(l => l.Length == 2 ? (l[0], int.Parse(l[1])) : (l[0][..^1], 0))
            .Select<(string label, int focalLength), (string label, int focalLength, int hash)>(l => (l.label, l.focalLength, l.label.Aggregate(0, (hash, c) => (hash + c) * 17 % 256)))
            .Aggregate(new List<(string, int)>[256], (hashmap, line) =>
                (hashmap[line.hash] = (new List<(string, int)>[] { hashmap[line.hash] ?? (hashmap[line.hash] = []) }
                    .Select(box =>
                        line.focalLength > 0
                            ? new int[] { box.Select((l, i) => (l, i)).FirstOrDefault(l => l.Item1.Item1 == line.label, (("", 0), -1)).Item2 }
                                .Select(index => index == -1 ? [.. box, (line.label, line.focalLength)] : (box[index] = (line.label, line.focalLength)) == default ? box : box)
                                .First()
                            : box.Remove(box.FirstOrDefault(l => l.Item1 == line.label)) ? box : box))
                    .First()) == default ? hashmap : hashmap)
            .Select((b, i) => b is null ? 0 : b.Select((l, j) => (i + 1) * (j + 1) * l.Item2).Sum()).Sum()}";
}