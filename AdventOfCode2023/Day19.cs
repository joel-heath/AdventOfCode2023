namespace AdventOfCode2023;
public class Day19 : IDay
{
    public int Day => 19;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        { "px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}", "19114" }
    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        { "px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}", "167409079868000" }
    };

    private static readonly char[] categories = ['x', 'm', 'a', 's'];

    private class Workflow
    {
        public string Name { get; }
        public int MachinePart { get; } // basically enum Part { X, M, A, S }
        public long CriticalValue { get; }
        public bool GreaterThan { get; } // otherwise less than
        public string True { get; }
        public object False => falseWorkflow is null ? falseString : falseWorkflow;

        private readonly Workflow? falseWorkflow;
        private readonly string falseString;

        public bool Compare(long value) => GreaterThan ? value > CriticalValue : value < CriticalValue;

        private Workflow(string name, int machinePart, long criticalValue, bool greaterThan, string ifTrue, string falseStr, Workflow? falseWork)
            => (Name, MachinePart, CriticalValue, GreaterThan, True, falseString, falseWorkflow) = (name, machinePart, criticalValue, greaterThan, ifTrue, falseStr, falseWork);
        
        public Workflow(string name, int machinePart, long criticalValue, bool greaterThan, string ifTrue, Workflow ifFalse)
            : this(name, machinePart, criticalValue, greaterThan, ifTrue, string.Empty, ifFalse) { }

        public Workflow(string name, int machinePart, long criticalValue, bool greaterThan, string ifTrue, string ifFalse)
            : this(name, machinePart, criticalValue, greaterThan, ifTrue, ifFalse, null) { }
    }

    public string SolvePart1(string input)
        => $"{new string[][] { input.Split(Environment.NewLine + Environment.NewLine) }.Select(blocks => (workflows: ParseWorkflows(blocks[0].Split(Environment.NewLine)), ratings: blocks[1])).Select(d => 
            d.ratings.Split(Environment.NewLine).Sum(l =>
                new long[][] { Utils.GetLongs(l).ToArray() }.Select(nums =>
                (Utils.EnumerateForever().AggregateWhile(
                    (workflowName: "in", workflow: d.workflows["in"]),
                    (acc, _) => acc.workflow.Compare(nums[(int)acc.workflow.MachinePart])
                        ? (acc.workflow.True,
                            d.workflows.TryGetValue(acc.workflow.True, out var newWorkflow1)
                                ? newWorkflow1 : acc.workflow)
                        : acc.workflow.False is Workflow newWorkflow
                            ? (acc.workflowName, newWorkflow)
                            : ((string)acc.workflow.False,
                                d.workflows.TryGetValue((string)acc.workflow.False, out var newWorkflow2)
                                    ? newWorkflow2 : acc.workflow)
                    , acc => acc.workflowName != "A" && acc.workflowName != "R").workflowName == "A")
                ? nums.Sum() : 0).Sum())).First()}";

    public string SolvePart2(string input)
        => $"{new string[] { input.Split(Environment.NewLine + Environment.NewLine)[0] }.Select(temp => ParseWorkflows(temp.Split(Environment.NewLine)))
            .Select(workflows => CountPossibilites(new (long, long)[] { (1, 4000), (1, 4000), (1, 4000), (1, 4000) }, workflows["in"], workflows)).First()}";

    static Dictionary<string, Workflow> ParseWorkflows(string[] lines)
    => lines.Select(l => l.Split('{')).Select(l => (name: l[0], things: l[1][..^1].Split(':').SelectMany(x => x.Split(',')).ToArray()))
        .ToDictionary(k => k.name, k =>
            Utils.Range(k.things.Length - 5, (k.things.Length - 3) / 2, -2)
                .Aggregate(
                    new string[][] { k.things[^3].Split('<').SelectMany(x => x.Split('>')).ToArray() }.Select(condition =>
                    new Workflow(k.name, Array.IndexOf(categories, condition[0][0]), int.Parse(condition[1]), k.things[^3].Contains('>'), k.things[^2], k.things[^1])).First()
                    , (prev, j) =>
                    new string[][] { k.things[j].Split('<').SelectMany(x => x.Split('>')).ToArray() }.Select(conditionSplit =>
                    new Workflow(k.name, Array.IndexOf(categories, conditionSplit[0][0]), int.Parse(conditionSplit[1]), k.things[j].Contains('>'), k.things[j + 1], prev)).First()
                ));

    static long CountPossibilites((long start, long end)[] ranges, Workflow workflow, Dictionary<string, Workflow> workflows)
    => new (long start, long end)[] { UpdateRange(ranges[workflow.MachinePart], workflow.CriticalValue, workflow.GreaterThan) }.Sum(newRange =>
        (newRange.start < newRange.end)
        ? new (long start, long end)[][] { InsertRange(ranges, workflow.MachinePart, newRange) }.Sum(newRanges =>
            workflows.TryGetValue(workflow.True, out var newWorkflow)
                ? CountPossibilites(newRanges, newWorkflow, workflows)
                : (workflow.True == "A") ? newRanges.Aggregate(1L, (p, r) => p * (r.end - r.start + 1)) : 0) : 0)

        + new (long start, long end)[] { UpdateRange(ranges[workflow.MachinePart], workflow.CriticalValue + (workflow.GreaterThan ? 1 : -1), !workflow.GreaterThan) }.Sum(newRange =>
            newRange.start < newRange.end
            ? new (long start, long end)[][] { InsertRange(ranges, workflow.MachinePart, newRange) }.Sum(newRanges =>
                (workflow.False is Workflow newWorkflow)
                    ? CountPossibilites(newRanges, newWorkflow, workflows)
                    : ((string)workflow.False == "A")
                        ? newRanges.Aggregate(1L, (p, r) => p * (r.end - r.start + 1))
                        : ((string)workflow.False != "R")
                                ? CountPossibilites(newRanges, workflows[(string)workflow.False], workflows) : 0) : 0);

    static (long start, long end) UpdateRange((long start, long end) range, long criticalValue, bool greaterThan)
        => greaterThan
            ? (criticalValue + 1 > range.start) ? (criticalValue + 1, range.end) : range
            : (criticalValue - 1 < range.end) ? (range.start, criticalValue - 1) : range;

    static (long start, long end)[] InsertRange((long, long)[] ranges, int index, (long, long) newRange)
        => index == 0 ? [newRange, ..ranges[1..]] : (index == ranges.Length - 1) ? [.. ranges[..^1], newRange] : [.. ranges[..index], newRange, .. ranges[(index + 1)..]];
}