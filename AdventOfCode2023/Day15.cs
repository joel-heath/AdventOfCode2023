namespace AdventOfCode2023;
public class Day15 : IDay
{
    public int Day => 15;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7",
            "1320"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7",
            "145"
        },

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
        var hashmap = new Dictionary<long, List<(string, long)>>();

        
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

            List<(string, long)> box = [];
            if (hashmap.TryGetValue(hash, out var oldbox))
            {
                box = oldbox;
            }
            hashmap[hash] = box;

            if (line.Contains('='))
            {
                var data = line.Split('=');
                //var label = data[0];
                var focalLength = long.Parse(data[1]);

                try
                {
                    var index = box.Select((l, i) => (l, i)).First(l => l.l.Item1 == label).i;
                    box[index] = (label, focalLength);
                }
                catch
                {
                    box.Add((label, focalLength));
                }
            }
            else
            {
                //var label = line[..^1];

                try
                {
                    box.Remove(box.First(l => l.Item1 == label));
                }
                catch { }
            }
        }

        long total = 0;
        foreach (var kvp in hashmap)
        {
            for (int i = 0; i < kvp.Value.Count; i++)
            {
                total += (kvp.Key + 1) * (i + 1) * (kvp.Value[i].Item2);
            }
        }

        return $"{total}";
    }
}