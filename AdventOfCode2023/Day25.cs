using NEAConsole.Matrices;

namespace AdventOfCode2023;
public class Day25 : IDay
{
    public int Day => 25;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "jqt: rhn xhk nvd\r\nrsh: frs pzl lsr\r\nxhk: hfx\r\ncmg: qnr nvd lhk bvb\r\nrhn: xhk bvb hfx\r\nbvb: xhk hfx\r\npzl: lsr hfx nvd\r\nqnr: nvd\r\nntq: jqt hfx bvb xhk\r\nnvd: lhk\r\nlsr: lhk\r\nrzs: qnr cmg lsr rsh\r\nfrs: qnr lhk lsr",
            "54"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "TestInput1",
            "ExpectedOutput1"
        },

    };

    public string SolvePart1(string input)
    {
        long total = 0;

        var allNums = Utils.GetLongs(input).ToArray();

        var (graphList, map) = Utils.GenerateGraph(input.Split(Environment.NewLine).SelectMany(line => line.Split(' ')[1..].Select(e => (line.Split(':')[0], e, 1))));
        var (graphDirected, _) = Utils.GenerateGraph(input.Split(Environment.NewLine).SelectMany(line => line.Split(' ')[1..].Select(e => (line.Split(':')[0], e, 1))), true);

        //var graph = graphList.ToDictionary(i => i.name, i => i.edges);

        //var adj = GetAdjacency(graphList);
        //var mst = MatrixUtils.Prims(adj);

        long max = 0;
        // 14, 9


        HashSet<int> sideOne = new();
        HashSet<int> sideTwo = new();

        Queue<int> remaining = new(Enumerable.Range(0, graphList.Count));

        while (true)
        {
            var a = remaining.Dequeue();
            var b = remaining.Dequeue();

            var aPoints = BFS(a, graphList);
            var bPoints = BFS(b, graphList);
            if (!aPoints.Intersect(bPoints).Any())
            {

            }
            
        }


        /*
        for (int i = 0; i < graphDirected.Count; i++)
        {
            foreach (var ei in graphDirected[i].edges.Keys)
            {
                var edgesFrom1 = graphList[i].edges;
                var edgesTo1 = graphList[ei].edges;

                edgesFrom1.Remove(ei);
                edgesTo1.Remove(i);

                for (int j = 0; j < graphDirected.Count; j++)
                {

                    foreach (var ej in graphDirected[j].edges.Keys)
                    {
                        if (j == i && ei == ej) continue;

                        var edgesFrom2 = graphList[j].edges;
                        var edgesTo2 = graphList[ej].edges;

                        edgesFrom2.Remove(ej);
                        edgesTo2.Remove(j);

                        for (int k = 0; k < graphDirected.Count; k++)
                        {

                            foreach (var ek in graphDirected[k].edges.Keys)
                            {
                                if (k == i && ei == ek || k == j && ej == ek) continue;

                                var edgesFrom3 = graphList[k].edges;
                                var edgesTo3 = graphList[ek].edges;
                                
                                edgesFrom3.Remove(ek);
                                edgesTo3.Remove(k);

                                var a = BFS(i, graphList);
                                var b = BFS(ei, graphList);


                                var ans = a * b;

                                if (a + b == 15 && ans > max)
                                {
                                    max = ans;
                                }

                                edgesFrom3.Add(ek, 1);
                                edgesTo3.Add(k, 1);
                            }
                        }

                        edgesFrom2.Add(ej, 1);
                        edgesTo2.Add(j, 1);
                    }
                }

                edgesFrom1.Add(ei, 1);
                edgesTo1.Add(i, 1);
            }
        }


        return $"{max}";
    }*/
    }

        private static HashSet<int> BFS(int start, List<(string name, Dictionary<int, int> edges)> graph)
        {
            HashSet<int> visited = [];
            Queue<int> toVisit = new([start]);

            long count = 0;
            while (toVisit.TryDequeue(out int index))
            {
                var node = graph[index];
                count++;
                visited.Add(index);

                foreach (var edge in node.edges.Keys.Where(e => !visited.Contains(e) && !toVisit.Contains(e)))
                {
                    toVisit.Enqueue(edge);
                }
            }

            return visited;
        }

    private static Matrix GetAdjacency(List<(string name, Dictionary<int, int> edges)> graph)
    {
        Matrix m = new(graph.Count);

        for (int i = 0; i < graph.Count; i++)
        {
            foreach (var edge in graph[i].edges)
            {
                m[i, edge.Key] = edge.Value;
            }
        }

        return m;
    }

    public string SolvePart2(string input)
    {



        return string.Empty;
    }
}