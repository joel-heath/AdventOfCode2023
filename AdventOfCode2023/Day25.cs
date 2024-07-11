namespace AdventOfCode2023;
public class Day25 : IDay
{
    public int Day => 25;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "jqt: rhn xhk nvd\r\nrsh: frs pzl lsr\r\nxhk: hfx\r\ncmg: qnr nvd lhk bvb\r\nrhn: xhk bvb hfx\r\nbvb: xhk hfx\r\npzl: lsr hfx nvd\r\nqnr: nvd\r\nntq: jqt hfx bvb xhk\r\nnvd: lhk\r\nlsr: lhk\r\nrzs: qnr cmg lsr rsh\r\nfrs: qnr lhk lsr", "54" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "TestInput1", "" }
    };

    public List<string> FindPath(string start, string end, Dictionary<string, List<string>> graph)
    {
        Dictionary<string, string> visited = []; // node to the node it came from
        Queue<(string, string)> workingValues = new([(start, string.Empty)]);

        while (workingValues.TryDequeue(out var stuff))
        {
            var (node, source) = stuff;
            visited.Add(node, source);
            if (node == end) break;

            foreach (var connection in graph[node].Where(n => !visited.ContainsKey(n)))
            {
                if (!workingValues.Any(v => v.Item1 == connection))
                {
                    workingValues.Enqueue((connection, node));
                }
            }
        }

        var curr = end;
        List<string> path = [curr];
        while (curr != start)
        {
            curr = visited[curr];
            path.Add(curr);
        }

        return path;
    }

    public IEnumerable<int> GetComponentSizes(Dictionary<string, List<string>> graph)
    {
        var subGraph = graph.ToDictionary();
        Queue<string> toVisit = new([graph.First().Key]);

        while (toVisit.TryDequeue(out var node))
        {
            subGraph.Remove(node);
            foreach (var connection in graph[node].Where(n => subGraph.ContainsKey(n) && !toVisit.Contains(n)))
            {
                toVisit.Enqueue(connection);
            }
        }

        var componentSize = graph.Count - subGraph.Count;
        yield return componentSize;

        if (subGraph.Count == 0)
        {
            yield break;
        }

        graph = subGraph;
        subGraph = graph.ToDictionary();

        foreach (var subgraph in GetComponentSizes(graph))
        {
            yield return subgraph;
        }
    }

    public string SolvePart1(string input)
    {
        long total = 0;

        HashSet<string> components = [];
        Dictionary<string, List<string>> wires = [];

        foreach (var line in input.Split(Environment.NewLine))
        {
            var sides = line.Split(": ");
            var name = sides[0];
            components.Add(name);
            var connectedNodes = sides[1].Split(' ');

            var arcList = wires.TryGetValue(name, out var oldArcs) ? oldArcs : [];

            foreach (var node in connectedNodes) arcList.Add(node);

            // graph is undirected, add the arc the other way round
            foreach (var node in connectedNodes)
            {
                var otherArcList = wires.TryGetValue(node, out var temp) ? temp : [];
                otherArcList.Add(name);
                wires[node] = otherArcList;
            }

            wires[name] = arcList;
        }

        var wirelist = components.ToList();

        Dictionary<(string, string), int> arcUsage = [];
        for (int i = 0; i < 500; i++)
        {
            var start = wirelist[Random.Shared.Next(0, wirelist.Count)];
            var end = wirelist[Random.Shared.Next(0, wirelist.Count)];

            foreach (var arc in FindPath(start, end, wires).Window(2))
            {
                var nodes = arc.OrderBy(i => i).ToArray(); // need some sort of way to say arcs a -> b and b -> a are the same, only storing the alphebetically ordered arcs will suffice
                if (arcUsage.ContainsKey((nodes[0], nodes[1])))
                {
                    arcUsage[(nodes[0], nodes[1])]++;
                }
                else
                {
                    arcUsage[(nodes[0], nodes[1])] = 1;
                }

            }
        }

        List<(string, string)> arcs = [.. arcUsage.OrderByDescending(a => a.Value).Select(x => x.Key)];

        arcUsage.Dump();

        foreach (var cuts in arcs.Take(10).Combinations(3))
        {
            var newWires = wires.ToDictionary();

            foreach (var cut in cuts)
            {
                newWires[cut.Item1].Remove(cut.Item2);
                newWires[cut.Item2].Remove(cut.Item1);
            }

            var componentSizes = GetComponentSizes(newWires).ToArray();

            if (componentSizes.Length == 2)
            {
                return $"{componentSizes[0] * componentSizes[1]}";
            }
        }

        return "Failed to find solution in estimated range";
    }

    public string SolvePart2(string input) => string.Empty;
}