namespace AdventOfCode2023;
public class Day19 : IDay
{
    public int Day => 19;
    public Dictionary<string, string> UnitTestsP1 => new()
    {
        {
            "px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}",
            "19114"
        },

    };
    public Dictionary<string, string> UnitTestsP2 => new()
    {
        {
            "px{a<2006:qkq,m>2090:A,rfg}\r\npv{a>1716:R,A}\r\nlnx{m>1548:A,A}\r\nrfg{s<537:gd,x>2440:R,A}\r\nqs{s>3448:A,lnx}\r\nqkq{x<1416:A,crn}\r\ncrn{x>2662:A,R}\r\nin{s<1351:px,qqz}\r\nqqz{s>2770:qs,m<1801:hdj,R}\r\ngd{a>3333:R,R}\r\nhdj{m>838:A,pv}\r\n\r\n{x=787,m=2655,a=1222,s=2876}\r\n{x=1679,m=44,a=2067,s=496}\r\n{x=2036,m=264,a=79,s=2244}\r\n{x=2461,m=1339,a=466,s=291}\r\n{x=2127,m=1623,a=2188,s=1013}",
            "167409079868000"
        },

    };

    enum Part { X, M, A, S };

    class Workflow
    {
        public string Name { get; set; }
        public Part MachinePart { get; set; }
        public long CriticalValue { get; set; }
        public bool GreaterThan { get; set; } // otherwise less than
        
        public string True { get; set; }
        public object False => falseWorkflow is null ? falseString : falseWorkflow;

        private readonly Workflow? falseWorkflow;
        private readonly string falseString;

        public bool Compare(long value)
            => GreaterThan ? value > CriticalValue : value < CriticalValue;

        public Workflow(string name, Part machinePart, long criticalValue, bool greaterThan, string ifTrue, Workflow ifFalse)
        {
            Name = name;
            MachinePart = machinePart;
            CriticalValue = criticalValue;
            GreaterThan = greaterThan;
            True = ifTrue;
            falseWorkflow = ifFalse;
            falseString = string.Empty;
        }
        public Workflow(string name, Part machinePart, long criticalValue, bool greaterThan, string ifTrue, string ifFalse)
        {
            Name = name;
            MachinePart = machinePart;
            CriticalValue = criticalValue;
            GreaterThan = greaterThan;
            True = ifTrue;
            falseString = ifFalse;
            falseWorkflow = null;
        }
    }

    public string SolvePart1(string input)
    {
        var temp = input.Split(Environment.NewLine + Environment.NewLine);
        var workflowLines = temp[0].Split(Environment.NewLine);
        var ratingLines = temp[1].Split(Environment.NewLine);

        char[] categories = ['x', 'm', 'a', 's'];

        Dictionary<string, Workflow> workflows = new();

        for (int i = 0; i < workflowLines.Length; i++)
        {
            var line = workflowLines[i].Split('{');
            var name = line[0];

            var things = line[1][..^1].Split(':').SelectMany(x => x.Split(',')).ToArray();

            string right = things[^1];
            string left = things[^2];
            var conditionRaw = things[^3];
            var conditionMediumRare = conditionRaw.Split('<').SelectMany(x => x.Split('>')).ToArray();

            Workflow prev = new(name, (Part)Array.IndexOf(categories, conditionMediumRare[0][0]), int.Parse(conditionMediumRare[1]), conditionRaw.Contains('>'), left, right);

            for (int j = things.Length - 5; j >= 0; j -= 2)
            {
                string ifTrue = things[j + 1];
                var condition = things[j];
                var conditionSplit = condition.Split('<').SelectMany(x => x.Split('>')).ToArray();
                var part = (Part)Array.IndexOf(categories, conditionSplit[0][0]);

                prev = new(name, part, int.Parse(conditionSplit[1]), condition.Contains('>'), ifTrue, prev);
            }
            
            workflows[name] = prev;
        }

        long total = 0;
        for (int i = 0; i < ratingLines.Length; i++)
        {
            string workflowName = "in";
            var nums = Utils.GetLongs(ratingLines[i]).ToArray();
            var workflow = workflows[workflowName];

            while (workflowName != "A" && workflowName != "R")
            {
                long count = nums[(int)workflow.MachinePart];
                
                if (workflow.Compare(count))
                {
                    workflowName = workflow.True;
                    if (workflows.TryGetValue(workflowName, out var newWorkflow1)) workflow = newWorkflow1;
                }
                else
                {
                    if (workflow.False is Workflow newWorkflow)
                        workflow = newWorkflow;
                    else
                    {
                        workflowName = (string)workflow.False;
                        if (workflows.TryGetValue(workflowName, out var newWorkflow2)) workflow = newWorkflow2;
                    }
                }
            }

            if (workflowName == "A")
            {
                total += nums.Sum().Dump();
            }
        }


        return $"{total}";
    }

    public string SolvePart2(string input)
    {
        var temp = input.Split(Environment.NewLine + Environment.NewLine);
        var workflowLines = temp[0].Split(Environment.NewLine);

        char[] categories = ['x', 'm', 'a', 's'];

        Dictionary<string, Workflow> workflows = new();

        for (int i = 0; i < workflowLines.Length; i++)
        {
            var line = workflowLines[i].Split('{');
            var name = line[0];

            var things = line[1][..^1].Split(':').SelectMany(x => x.Split(',')).ToArray();

            string right = things[^1];
            string left = things[^2];
            var conditionRaw = things[^3];
            var conditionMediumRare = conditionRaw.Split('<').SelectMany(x => x.Split('>')).ToArray();

            Workflow prev = new(name, (Part)Array.IndexOf(categories, conditionMediumRare[0][0]), int.Parse(conditionMediumRare[1]), conditionRaw.Contains('>'), left, right);

            for (int j = things.Length - 5; j >= 0; j -= 2)
            {
                string ifTrue = things[j + 1];
                var condition = things[j];
                var conditionSplit = condition.Split('<').SelectMany(x => x.Split('>')).ToArray();
                var part = (Part)Array.IndexOf(categories, conditionSplit[0][0]);

                prev = new(name, part, int.Parse(conditionSplit[1]), condition.Contains('>'), ifTrue, prev);
            }

            workflows[name] = prev;
        }

        (long start, long end)[] ranges = [(1, 4000), (1, 4000), (1, 4000), (1, 4000)];

        string workflowName = "in";
        var workflow = workflows[workflowName];

        return $"{CountPossibilites(ranges, workflow, workflows)}";
    }

    static (long start, long end) UpdateRange((long start, long end) range, long criticalValue, bool greaterThan)
    {
        if (greaterThan)
        {
            if (criticalValue + 1 > range.start)
            {
                range.start = criticalValue + 1;
            }
            if (range.start > range.end) range = (0, 0);
        }
        else
        {
            if (criticalValue - 1 < range.end)
            {
                range.end = criticalValue - 1;
            }
            if (range.start > range.end) range = (0, 0);
        }

        return range;
    }

    static (long start, long end)[] InsertRange((long start, long end)[] ranges, int index, (long start, long end) newRange)
    {
        if (index == 0)
            return [newRange, ..ranges[1..]];
        if (index == ranges.Length - 1)
            return [.. ranges[..^1], newRange];
        return [.. ranges[..index], newRange, .. ranges[(index + 1)..]];
    }

    static long CountPossibilites((long start, long end)[] ranges, Workflow workflow, Dictionary<string, Workflow> workflows)
    {
        long total = 0;
        int part = (int)workflow.MachinePart;

        // if pass
        {
            var newRange = UpdateRange(ranges[part], workflow.CriticalValue, workflow.GreaterThan);
            if (newRange != (0, 0))
            {
                var newRanges = InsertRange(ranges, part, newRange);

                if (workflows.TryGetValue(workflow.True, out var newWorkflow))
                {
                    total += CountPossibilites(newRanges, newWorkflow, workflows);
                }
                else
                {
                    if (workflow.True == "A")
                        total += newRanges.Aggregate(1L, (p, r) => p * (r.end - r.start + 1));
                }
            }
        }
        // if fail
        {
            var newRange = UpdateRange(ranges[part], workflow.CriticalValue + (workflow.GreaterThan ? 1 : -1), !workflow.GreaterThan);
            if (newRange != (0, 0))
            {
                var newRanges = InsertRange(ranges, part, newRange);

                if (workflow.False is Workflow newWorkflow)
                {
                    total += CountPossibilites(newRanges, newWorkflow, workflows);
                }
                else
                {
                    var str = (string)workflow.False;
                    if (str == "A")
                        total += newRanges.Aggregate(1L, (p, r) => p * (r.end - r.start + 1));
                    else if (str != "R")
                    {
                        total += CountPossibilites(newRanges, workflows[str], workflows);
                    }
                }
            }
        }

        return total;
    }
}