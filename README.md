# Advent of Code 2023
My C# solutions to [Advent of Code 2023](https://adventofcode.com/2023). Trying to one-liner as many as possible.

## Notes
Here you can easily navigate each day's code & read about how well I think I did, as well as how close I was to solving it in one line.
One line doesn't necessarily mean as short as possible. Most the time getting it into one line makes it longer and much harder to read.
Since any one-liner calling another one-liner recursive function could be reduced to one line by adding the parameters to the calling function & defaulting them to null, I consider such solutions 'one-liner's.

### Legend
🟢 Successful one-liner.<br>
🟡 One-liner with up to three functions / class variables<br>
🟠 Far from a one-liner, with minimal amounts of code reduced.<br>
🔴 Barely a visible effort to reduce the code. <br>
⚫ Unattempted (probably because the problem isn't out yet, or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2023/Day01.cs) | 🟢 | Surprisingly complicated day one, involving the English numerals that overlap for part two. Simplest solution to part two is to replace `one` with `one1one` (etc.) to preserve overlapping lines. |
| [2](AdventOfCode2023/Day02.cs) | 🟢 | This one I went straight for regex, but the easier solution was splitting. I've got one-liners, but they're complicated ones today, making good use of `GroupBy()` |
| [3](AdventOfCode2023/Day03.cs) | 🟢 | Today's is a difficult one to reduce to one line as you need to search around a point, re-indexing the input. A simple solution is containerizing the input and iterating over that one-item collection. The input can then be parsed and then starting a new Select will allow the parsed input to be referenced. All that's needed at the end is a `.First()` |
| [4](AdventOfCode2023/Day04.cs) | 🟢 | Part one is an easy one-liner, but part two is another one that in theory requires re-indexing, but using an aggregate to carry along an array while calculating the total allows it to be done. I'm not sure the one-liners will last much longer... |
| [5](AdventOfCode2023/Day05.cs) | 🟢 | Part one was straightforward, but part two... Due to one range being mapped to many ranges, which each could be mapped to more, recursion is required. My initial solution passed in a list of remaining ranges, took the first item and recursed on index 1 onwards (concatenating with any more ranges generated), but this means each time you have to `ToList()` the output. Now I'm passing in an IEnumerator and using the fact `MoveNext()` returns a bool telling whether there are more items (the base case). |
| [6](AdventOfCode2023/Day06.cs) | 🟢 | A nice easy one to get us to forgive Eric for yesterday's. I solved this one by turning the problem into a quadratic in the time holding the button, then solving for f(x) > best. |
| [7](AdventOfCode2023/Day07.cs) | 🟢 | Today's wasn't too bad, the end goal was to calculate a unique score for each possible hand, and use `OrderBy()` to do the rest of the work. I achieved this by treating the cards as a base 14 (since number 1 is skipped) number system, then adding very large quantities for the number of duplicate card values. |
| [8](AdventOfCode2023/Day08.cs) | 🟡 | Part 2 is a classic lowest common multiple problem. Unfortunately today marks the end of the one-liner streak, due to multiple while loops which in one-liner world translate to recursion. However, after about 7000 recurses the program gives in. (I need 21,000+). Part 1 & 2 are one-liners, but I've got another function which is not one line long. |
| [9](AdventOfCode2023/Day09.cs) | 🟢 | Full one-liners today, which I was not expecting. I achieved it through using `Aggregate()` three times for part two. |
| [10](AdventOfCode2023/Day10.cs) | 🟡 | Somehow I've written two one-liners with two class variables & a breadth-first search. I'm genuinely astonished I did this part two is the most gargantuan one-liner I've ever written. Unlike some I chose to solve part two through PIP even-odd ray casting. Thing's get funny when a point shares a coordinate with both vertices of an edge. |
| [11](AdventOfCode2023/Day11.cs) | 🟢 | Again made use of my point class today, as well as a very helpful [combinations method](https://stackoverflow.com/a/33336576/13361257) to find all n(n+1)/2 different pairs of galaxies. |
| [12](AdventOfCode2023/Day12.cs) | 🟢 | Solved part 2 like many with memoization. Today's I've reduced to a one liner, but it increases the runtime from less than a second to about a second and a half, so though I consider this a win as I got a one-liner, I will keep my many-lined solution. |
| [13](AdventOfCode2023/Day13.cs) | 🟢 | Pretty nice part one & two, perfect for multiple use of LINQ's `All()` for part two I used the exact same code, but it stores whether it's found a smudge (an error), then if it finds a second one it returns false. |
| [14](AdventOfCode2023/Day14.cs) | ⚫ |  |
| [15](AdventOfCode2023/Day15.cs) | ⚫ |  |
| [16](AdventOfCode2023/Day16.cs) | ⚫ |  |
| [17](AdventOfCode2023/Day17.cs) | ⚫ |  |
| [18](AdventOfCode2023/Day18.cs) | ⚫ |  |
| [19](AdventOfCode2023/Day19.cs) | ⚫ |  |
| [20](AdventOfCode2023/Day20.cs) | ⚫ |  |
| [21](AdventOfCode2023/Day21.cs) | ⚫ |  |
| [22](AdventOfCode2023/Day22.cs) | ⚫ |  |
| [23](AdventOfCode2023/Day23.cs) | ⚫ |  |
| [24](AdventOfCode2023/Day24.cs) | ⚫ |  |
| [25](AdventOfCode2023/Day25.cs) | ⚫ |  |
