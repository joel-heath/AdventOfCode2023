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
    {
        long total = 0;

        var lines = input.Split(',');
        for (int i = 0; i < lines.Length; i++)
        {
            long hash = 0;
            var line = lines[i];

            for (int j = 0; j < line.Length; j++)
            {
                hash += line[j];
                hash *= 17;
                hash %= 256;
            }

            total += hash;
        }


        return $"{total}";
    }

    public string SolvePart2(string input)
    {
        var hashmap = new List<(string, int)>[256];

        var lines = input.Split(',');
        for (int i = 0; i < lines.Length; i++)
        {            
            var line = lines[i];
            var label = string.Concat(line.TakeWhile(c => c != '-' && c != '='));
            long hash = 0;
            for (int j = 0; j < label.Length; j++)
            {
                hash += label[j];
                hash *= 17;
                hash %= 256;
            }
            var box = hashmap[hash] ?? (hashmap[hash] = []);

            if (line.Contains('='))
            {
                var data = line.Split('=');
                var focalLength = int.Parse(data[1]);

                var index = box.Select((l, i) => (l, i)).FirstOrDefault(l => l.Item1.Item1 == label, (("", 0), -1)).Item2;
                if (index == -1) box.Add((label, focalLength));
                else box[index] = (label, focalLength);
            }
            else
            {
                box.Remove(box.FirstOrDefault(l => l.Item1 == label));
            }
        }

        long total = 0;

        for (int i = 0; i < hashmap.Length; i++)
        {
            if (hashmap[i] is null) continue;
            for (int j = 0; j < hashmap[i].Count; j++)
            {
                total += (i + 1) * (j + 1) * hashmap[i][j].Item2;
            }
        }

        return $"{total}";
    }
}