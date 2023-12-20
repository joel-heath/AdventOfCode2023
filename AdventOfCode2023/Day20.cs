using System.Data;

namespace AdventOfCode2023;
public class Day20 : IDay
{
    public int Day => 20;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "broadcaster -> a, b, c\r\n%a -> b\r\n%b -> c\r\n%c -> inv\r\n&inv -> a", "32000000" },
        { "broadcaster -> a\r\n%a -> inv, con\r\n&inv -> b\r\n%b -> con\r\n&con -> output", "11687500" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "broadcaster -> a\r\n%a -> b\r\n&b -> rx", "1" }
    };

    public interface IModule
    {
        string Name { get; }
        string[] Outputs { get; }
        IEnumerable<(string, bool)> Process(string source, bool pulse);
    }

    class Broadcaster(string name, string[] outputs) : IModule
    {
        public string Name { get; } = name;
        public string[] Outputs { get; } = outputs;
        public IEnumerable<(string, bool)> Process(string source, bool pulse) => Outputs.Select(node => (node, pulse));
    }


    class FlipFlop(string name, string[] outputs) : IModule
    {
        private bool state = false;
        public string Name { get; } = name;
        public string[] Outputs { get; } = outputs;
        public IEnumerable<(string, bool)> Process(string source, bool pulse)
            => !pulse ? (state = !state) ? Outputs.Select(node => (node, state)) : Outputs.Select(node => (node, state)) : Array.Empty<(string, bool)>();
    }

    class Conjunction(string name, string[] outputs) : IModule
    {
        private readonly Dictionary<string, bool> history = [];
        public string Name { get; } = name;
        public string[] Outputs { get; } = outputs;
        public string[] Inputs => history.Keys.ToArray();
        public bool AddInput(string input) => history[input] = false;
        public IEnumerable<(string, bool)> Process(string source, bool pulse)
            => (pulse = ((history[source] = pulse) || true) && !history.Values.All(i => i)) ? Outputs.Select(node => (node, pulse)) : Outputs.Select(node => (node, pulse));
    }

    public string SolvePart1(string input)
        => $"{new Dictionary<string, IModule>[] { input.Split(Environment.NewLine).Select(l => new string[][] { l.Split(" -> ") }.Select(l => (l[0], l[0].Split("%").SelectMany(i => i.Split("&")).Last(), l[1].Split(", ").ToArray())).First())
            .ToDictionary<(string, string, string[]), string, IModule>(l => l.Item2, l => l.Item1.Contains('%') ? new FlipFlop(l.Item2, l.Item3) : l.Item1.Contains('&') ? new Conjunction(l.Item2, l.Item3) : new Broadcaster(l.Item2, l.Item3)) }
            .Select(graph => graph.Where(kvp => kvp.Value is Conjunction).All(con => 
                new Conjunction[] { (con.Value as Conjunction)! }.Select(conjunction => 
                graph.Where(kvp => kvp.Value.Outputs.Contains(conjunction.Name)).Select(node => conjunction.AddInput(node.Key)).ToArray().Length == 1).First() || true) ? graph : graph)
            .Sum(graph => new (int highCount, int lowCount)[] { Enumerable.Range(0, 1000).Aggregate((highs: 0, lows: 0), (grandAcc, _) =>
                new (int high, int low, object)[] { Utils.EnumerateForever().AggregateWhile((highs: 0, lows: 1, queue:new Queue<(IModule, bool, string)>([(graph.First(i => i.Value is Broadcaster).Value, false, "button")])), (acc, _) =>
                    acc.queue.TryDequeue(out var item) ? item.Item1.Process(item.Item3, item.Item2)
                        .Aggregate(acc, (acc, output) => (graph.TryGetValue(output.Item1, out var value) ? new Action(() => acc.queue.Enqueue((value, output.Item2, item.Item1.Name))).InvokeTruthfully() : true)
                                            ? output.Item2 ? (acc.highs + 1, acc.lows, acc.queue) : (acc.highs, acc.lows + 1, acc.queue) : acc) : acc
                        , acc => acc.queue.Count > 0) }
                    .Select(i => (grandAcc.highs + i.high, grandAcc.lows + i.low)).First()) }.Sum(i => i.highCount * i.lowCount))}";

    public string SolvePart2(string input)
        => $"{new Dictionary<string, IModule>[] { input.Split(Environment.NewLine).Select(l => new string[][] { l.Split(" -> ") }.Select(l => (l[0], l[0].Split("%").SelectMany(i => i.Split("&")).Last(), l[1].Split(", ").ToArray())).First())
                .ToDictionary<(string, string, string[]), string, IModule>(l => l.Item2, l => l.Item1.Contains('%') ? new FlipFlop(l.Item2, l.Item3) : l.Item1.Contains('&') ? new Conjunction(l.Item2, l.Item3) : new Broadcaster(l.Item2, l.Item3)) }
            .Select(graph => new Action(() => graph.Add("rx", new Conjunction("rx", []))).InvokeTruthfully() ? graph : graph)
            .Select(graph => graph.Where(kvp => kvp.Value is Conjunction).All(con =>
                new Conjunction[] { (con.Value as Conjunction)! }.Select(conjunction =>
                graph.Where(kvp => kvp.Value.Outputs.Contains(conjunction.Name)).Select(node => conjunction.AddInput(node.Key)).ToArray().Length == 1).First() || true) ? graph : graph)
            .Select(graph => (graph, inputs: (graph.First(i => i.Key == "rx").Value as Conjunction)!.Inputs.SelectMany(i => (graph[i] as Conjunction)!.Inputs).ToHashSet(), counts: new List<long>()))
            .Select(data => Utils.EnumerateForever().AggregateWhile(1, (count, _) => new Queue<(IModule, bool, string)>[] { new([(data.graph.First(i => i.Value is Broadcaster).Value, false, "button")]) }
                .Select(queue => Utils.EnumerateForever().TakeWhile(x => queue.TryDequeue(out var item)
                && item.Item1.Process(item.Item3, item.Item2).Select(output => !output.Item2 && data.inputs.Contains(output.Item1) && !new Action(() => data.counts.Add(count)).InvokeTruthfully()
                        || !data.graph.TryGetValue(output.Item1, out var value) || new Action(() => queue.Enqueue((value, output.Item2, item.Item1.Name))).InvokeTruthfully()).All(x => x)).All(x => true))
            .First() ? count + 1 : count + 1
            , count => data.counts.Count < data.inputs.Count) == 1 ? Utils.LCM(data.counts) : Utils.LCM(data.counts)).First()}";
}