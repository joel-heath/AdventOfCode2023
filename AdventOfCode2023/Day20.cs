using System.Data;

namespace AdventOfCode2023;
public class Day20 : IDay
{
    public int Day => 20;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "broadcaster -> a\r\n%a -> inv, con\r\n&inv -> b\r\n%b -> con\r\n&con -> output",
            "11687500"
        },
        {
            "broadcaster -> a, b, c\r\n%a -> b\r\n%b -> c\r\n%c -> inv\r\n&inv -> a",
            "32000000"
        },
    };
    public Dictionary<string, string> UnitTestsP2 => new() { };

    //enum Module { Broadcaster, FlipFlop, Conjuction }
    public interface IModule
    {
        IEnumerable<(string, bool)> Execute(string source, bool pulse);
        string Name { get; }
        string State { get; }
        string[] Outputs { get; }
    }

    class FlipFlop : IModule
    {
        bool state;
        public string Name { get; }
        public string[] Outputs { get; }

        public string State { get => $"{Name}:{state}"; }

        public IEnumerable<(string, bool)> Execute(string source, bool pulse)
        {
            if (!pulse)
            {
                state = !state;
                foreach (var node in Outputs)
                {
                    yield return (node, state);
                }
            }
        }

        public FlipFlop(string name, string[] outputs)
        {
            this.Name = name;
            this.Outputs = outputs;
            state = false;
        }
    }

    class Conjunction : IModule
    {
        public string Name { get; }
        private Dictionary<string, bool> history;
        public string[] Outputs { get; }

        public string State { get => $"{Name}:{{{string.Join(',', history.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}}}"; }

        public IEnumerable<(string, bool)> Execute(string source, bool pulse)
        {
            history[source] = pulse;
            bool output = !history.Values.All(i => i);

            foreach (var node in Outputs)
            {
                yield return (node, output);
            }
        }

        public void AddInput(string input) => history[input] = false;

        public string[] Inputs => history.Keys.ToArray();

        public Conjunction(string name, string[] outputs)
        {
            this.Name = name;
            Outputs = outputs;
            history = new();
        }
    }

    class Broadcaster : IModule
    {
        public string Name { get; }
        public string[] Outputs { get; }

        public string State { get => string.Empty; }

        public IEnumerable<(string, bool)> Execute(string source, bool pulse)
        {
            foreach (var node in Outputs)
            {
                yield return (node, pulse);
            }
        }

        public Broadcaster(string name, string[] outputs)
        {
            this.Name = name;
            this.Outputs = outputs;
        }
    }

    public string SolvePart1(string input)
    {
        Dictionary<string, IModule> graph = new();

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(" -> ");
            var firstHalf = line[0].Split("%").SelectMany(i => i.Split("&")).ToArray();
            var inputName = firstHalf[^1];
            var outputs = line[1].Split(", ").ToArray();

            graph[inputName] =
                line[0].Contains("%") ? new FlipFlop(inputName, outputs)
                : line[0].Contains("&") ? new Conjunction(inputName, outputs)
                : new Broadcaster(inputName, outputs);
        }

        foreach (var con in graph.Where(kvp => kvp.Value is Conjunction))
        {
            var conjunction = (con.Value as Conjunction)!;
            foreach (var node in graph.Where(kvp => kvp.Value.Outputs.Contains(conjunction.Name)))
            {
                conjunction.AddInput(node.Key);
            }
        }

        var button = graph.First(i => i.Value is Broadcaster).Value;
        List<(string state, int highs, int lows)> stateHistory = new();
        int lb = -1;
        int count = 0;
        int total = 1000;

        while (count < total) //lb < 0 && count < total)
        {
            //push button
            int highs = 0, lows = 1; //otuput to broadcast
            Queue<(IModule, bool, string)> queue = new([(button, false, "button")]);

            //Console.WriteLine("button -low-> broadcaster");
            while (queue.TryDequeue(out var item))
            {
                foreach (var output in item.Item1.Execute(item.Item3, item.Item2))
                {
                    //Console.WriteLine($"{item.Item1.Name} -{(output.Item2 ? "high" : "low")}-> {output.Item1}");
                    if (graph.TryGetValue(output.Item1, out var value)) queue.Enqueue((value, output.Item2, item.Item1.Name));
                    if (output.Item2) highs++;
                    else lows++;
                }
            }


            var state = string.Join(';', graph.Select(i => i.Value.State).Concat(queue.Select(i => $"{i.Item1.Name}:{i.Item2}")));
            lb = stateHistory.Select((s, i) => (s, i)).FirstOrDefault(h => h.Item1.Item1 == state, ((string.Empty, 0, 0), -1)).Item2;
            stateHistory.Add((state, highs, lows));
            count++;
        }

        return $"{stateHistory.Sum(x => x.lows) * stateHistory.Sum(x => x.highs)}";
        if (lb == -1)
        {
            return $"{stateHistory.Sum(x => x.lows) * stateHistory.Sum(x => x.highs)}";

        }


        long totalLows = 0;
        long totalHighs = 0;

        if (lb > 0)
        {
            var preceding = stateHistory[..lb];
            totalLows += preceding.Sum(x => x.lows);
            totalHighs += preceding.Sum(x => x.highs);
        }

        total -= lb;

        var repeats = stateHistory[lb..^1];
        var k = total / repeats.Count;
        //$"{repeats.Sum(x => x.lows)} * {k} = {repeats.Sum(x => x.lows) * k}".Dump();
        //$"{repeats.Sum(x => x.highs)} * {k} = {repeats.Sum(x => x.highs) * k}".Dump();
        //var total = repeats.Sum(x => x.lows) * k * repeats.Sum(x => x.highs) * k;
        //var total = k * k * repeats.Sum(x => x.highs * x.lows);

        totalLows += k * repeats.Sum(x => x.lows);
        totalHighs += k * repeats.Sum(x => x.highs);

        if (total % repeats.Count > 0)
        {
            var proceeding = repeats[..(total % repeats.Count)];
            
            totalLows += proceeding.Sum(x => x.lows);
            totalHighs += proceeding.Sum(x => x.highs);
        }

        return $"{totalLows * totalHighs}";
    }

    public string SolvePart2(string input)
    {
        Dictionary<string, IModule> graph = new();

        var lines = input.Split(Environment.NewLine);
        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].Split(" -> ");
            var firstHalf = line[0].Split("%").SelectMany(i => i.Split("&")).ToArray();
            var inputName = firstHalf[^1];
            var outputs = line[1].Split(", ").ToArray();

            graph[inputName] =
                line[0].Contains("%") ? new FlipFlop(inputName, outputs)
                : line[0].Contains("&") ? new Conjunction(inputName, outputs)
                : new Broadcaster(inputName, outputs);
        }

        graph.Add("rx", new Conjunction("rx", []));

        foreach (var con in graph.Where(kvp => kvp.Value is Conjunction))
        {
            var conjunction = (con.Value as Conjunction)!;
            foreach (var node in graph.Where(kvp => kvp.Value.Outputs.Contains(conjunction.Name)))
            {
                conjunction.AddInput(node.Key);
            }
        }

        var button = graph.First(i => i.Value is Broadcaster).Value;

        // rx is a conjunction, all it's inputs must be 1
        // all of rx's inputs are conjunctions, all their inputs must be 0s
        var inputs = (graph.First(i => i.Key == "rx").Value as Conjunction)!.Inputs.SelectMany(i => (graph[i] as Conjunction)!.Inputs).ToHashSet();
        var counts = new List<long>();

        long count = 1;
        while (counts.Count < inputs.Count)
        {
            Queue<(IModule, bool, string)> queue = new([(button, false, "button")]);

            while (queue.TryDequeue(out var item))
            {
                foreach (var output in item.Item1.Execute(item.Item3, item.Item2))
                {
                    if (!output.Item2 && inputs.Contains(output.Item1))
                    {
                        counts.Add(count);
                    }
                    if (graph.TryGetValue(output.Item1, out var value))
                    {
                        queue.Enqueue((value, output.Item2, item.Item1.Name));
                    }
                }
            }

            count++;
        }

        return $"{Utils.LCM(counts)}";
    }
}