# Advent of Code 2023
My C# solutions to [Advent of Code 2023](https://adventofcode.com/2023). Trying to one-liner as many as possible.

## Notes
Here you can easily navigate each day's code & read about how well I think I did, as well as how close I was to solving it in one line.
One line doesn't necessarily mean as short as possible. Most the time getting it into one line makes it longer and much harder to read.
Since any one-liner calling another one-liner recursive function could be reduced to one line by adding the parameters to the calling function & defaulting them to null, I consider such solutions 'one-liner's.

### Legend
🟢 Successful one-liner.<br>
🟡 One-liner with other multiple one-liner methods & classes all of which only contain one-liner methods.<br>
🟠 One-liner with some functions that are more than one-line long.<br>
🔴 Far from a one-liner, with minimal amounts of code reduced.<br>
⚫ Unsolved (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2023/Day01.cs) | 🟢 | Surprisingly complicated day one, involving the English numerals that overlap for part two. Simplest solution to part two is to replace `one` with `one1one` (etc.) to preserve overlapping lines. |
| [2](AdventOfCode2023/Day02.cs) | 🟢 | This one I went straight for regex, but the easier solution was splitting. I've got one-liners, but they're complicated ones today, making good use of `GroupBy()` |
| [3](AdventOfCode2023/Day03.cs) | 🟢 | Today's is a difficult one to reduce to one line as you need to search around a point, re-indexing the input. A simple solution is containerizing the input and iterating over that one-item collection. The input can then be parsed and then starting a new Select will allow the parsed input to be referenced. All that's needed at the end is a `.First()` |
| [4](AdventOfCode2023/Day04.cs) | 🟢 | Part one is an easy one-liner, but part two is another one that in theory requires re-indexing, but using an aggregate to carry along an array while calculating the total allows it to be done. I'm not sure the one-liners will last much longer... |
| [5](AdventOfCode2023/Day05.cs) | 🟢 | Part one was straightforward, but part two... Due to one range being mapped to many ranges, which each could be mapped to more, recursion is required. My initial solution passed in a list of remaining ranges, took the first item and recursed on index 1 onwards (concatenating with any more ranges generated), but this means each time you have to `ToList()` the output. Now I'm passing in an IEnumerator and using the fact `MoveNext()` returns a bool telling whether there are more items (the base case). |
| [6](AdventOfCode2023/Day06.cs) | 🟢 | A nice easy one to get us to forgive Eric for yesterday's. I solved this one by turning the problem into a quadratic in the time holding the button, then solving for f(x) > best. |
| [7](AdventOfCode2023/Day07.cs) | 🟢 | Today's wasn't too bad, the end goal was to calculate a unique score for each possible hand, and use `OrderBy()` to do the rest of the work. I achieved this by treating the cards as a base 14 (since number 1 is skipped) number system, then adding very large quantities for the number of duplicate card values. |
| [8](AdventOfCode2023/Day08.cs) | 🟢 | Part 2 is a classic lowest common multiple problem, solve each individually and find their LCM. I'm using my custom `AggregateWhile` and `EnumerateForever` methods to make a LINQ-y way of doing while loops. |
| [9](AdventOfCode2023/Day09.cs) | 🟢 | Full one-liners today, which I was not expecting. I achieved it through using `Aggregate()` three times for part two. |
| [10](AdventOfCode2023/Day10.cs) | 🟠 | I chose to solve part two through PIP even-odd ray casting. This can be problematic when a point shares a coordinate with both vertices of an edge, but a simple rule of "if the edges go in different directions, count it once" fixes that. |
| [11](AdventOfCode2023/Day11.cs) | 🟢 | Again made use of my point class today, as well as a very helpful [combinations method](https://stackoverflow.com/a/33336576/13361257) to find all n(n+1)/2 different pairs of galaxies. |
| [12](AdventOfCode2023/Day12.cs) | 🟢 | Solved part 2 like many with memoization. Today's I've reduced to a one liner, but it increases the runtime from less than a second to about a second and a half, so though I consider this a win as I got a one-liner, I will keep my many-lined solution. |
| [13](AdventOfCode2023/Day13.cs) | 🟢 | Pretty nice part one & two, perfect for multiple use of LINQ's `All()` for part two I used the exact same code, but it stores whether it's found a smudge (an error), then if it finds a second one it returns false. |
| [14](AdventOfCode2023/Day14.cs) | 🟢 | Today's is (as is typical from myself) using impure LINQ methods with some very questionable approaches to forcing evaluation. My solution is `x => x.ToArray().Length == 0`, so the compiler can't cut any corners and has to evaluate every element of the array. |
| [15](AdventOfCode2023/Day15.cs) | 🟢 | A very simple part one following a primitive hashing algorithm, part two introduces some new features, but nothing aggregation can't handle. |
| [16](AdventOfCode2023/Day16.cs) | 🟢 | I solved today's using recursion & a sort-of memo (since I'm again using impure LINQ, if the current state is in the memo, it just returns). This one's also got some rather wild looking boolean operations & ternary ifs. |
| [17](AdventOfCode2023/Day17.cs) | 🟢 | Solved today's again using very impure LINQ with my `EnumerateForever` & `AggregateWhile` extension methods. The trick was using a priority queue based on the distance from the start node, and clever methods to reduce the search space.  |
| [18](AdventOfCode2023/Day18.cs) | 🟢 | Today's part two had me clueless for quite a while but through some research I discovered signed areas of sides, and the many formulae for the sum of signed areas. I chose the [triangle formula](https://en.wikipedia.org/wiki/Shoelace_formula#Triangle_formula) |
| [19](AdventOfCode2023/Day19.cs) | 🟡 | I gave in to OOP today & made a Workflow class with a discriminated union for the "if false" option. Solved part two by recursively counting options by refining ranges. |
| [20](AdventOfCode2023/Day20.cs) | 🟡 | Similarly to yesterday I object-oriented today's solution, yet somehow I still managed to make every function a one-liner so only yellow for today. For my input, `rx`'s inputs were all conjunctions (and hence all needed to be 1s) so I found when all of `rx`'s inputs' inputs were 0s, and calculated their LCM. |
| [21](AdventOfCode2023/Day21.cs) | 🟢 | I solved today's part two by noticing the empty "highways" on the starting position row, meaning you can directly get to a loop of the map, and massively reducing the problem. The map grows in a square so by finding the total number of reachable garden plots for 3 different numbers of repeats of the map, you can find the coefficients of a quadratic representing n repeats. Finally, since 26501365 = 131 * 202300 (grid size) + 65 (starting position), there are 131 repeats. |
| [22](AdventOfCode2023/Day22.cs) | 🟢 | My method for today's was simply to find all that are able to fall. In part one I remove these from my list, in part two I add them to a list. Unfortunately though my part two takes about a minute to run. |
| [23](AdventOfCode2023/Day23.cs) | 🔴 | Very disappointingly slow solution for part 2, mostly due to reaching stack overflows and having to create copies of sets as a result. |
| [24](AdventOfCode2023/Day24.cs) | 🔴 | My part two is not deterministic, but I solved it by generating i and j components of the direction vector, then manually solving simultaneous equations for the i & j components of the position vector, and separately the k components of both vectors. I then test to see if it intersects all lines and if not try another. |
| [25](AdventOfCode2023/Day25.cs) | 🔴 | Again, non-deterministic. I'm performing a BFS between 500 random pairs of nodes to find the most used arcs, then taking the top 10 of those and trying every combination of 3 of them until a cut is successfully made. |
