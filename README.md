# Advent of Code 2023
My C# solutions to [Advent of Code 2023](https://adventofcode.com/2023). Trying to one-liner as many as possible.

## Notes
Here you can easily navigate each days code and read about how well I think I did, as well as how close I was to solving it in one line.

### Legend
🟢 Successful one-liner.<br>
🟡 One-liner with up to three functions / class variables<br>
🟠 Far from a one-liner, with minimal amounts of code reduced.<br>
🔴 Barely a visible effort to reduce the code. <br>
⚫ Unattempted (probably because the problem isn't out yet or I forgot to push).

| **Day** | **Verbosity** | **Notes** |
|:---:|:---:|:---:|
| [1](AdventOfCode2023/Day01.cs) | 🟢 | Suprisingly complicated day one, involving the English numerals that overlap for part two. Simplest solution to part two is to replace `one` with `one1one` (etc.) to preserve overlapping lines. |
| [2](AdventOfCode2023/Day02.cs) | 🟢 | This one I went straight for regex, but the easier solution was splitting. I've got one-liners but they're complicated ones today, making good use of `GroupBy()` |
| [3](AdventOfCode2023/Day03.cs) | 🟢 | Today's is a difficult one to reduce to one line as you need to search around a point, re-indexing the input. A simple solution is containerizing the input and iterating over that one-item collection. The input can then be parsed and then starting a new Select will allow the parsed input to be referenced. All thats needed at the end is a `.First()` |
| [4](AdventOfCode2023/Day04.cs) | 🟢 | Part one is an easy one-liner, but part two is another one that in theory requires re-indexing, but using an aggregate to carry along an array while calculating the total allows it to be done. I'm not sure the one-liners will last much longer... |
| [5](AdventOfCode2023/Day05.cs) | 🟠 | I got part one down to one line but part two is proving very difficult, due to one range being mapped to many ranges, which each could be mapped to more a procedural approach is a queue and a function is recursive, neither of which can be done in one line. Besides, my recursive solution doesn't work. This may be the end of the line for the one-liners. |
| [6](AdventOfCode2023/Day06.cs) | ⚫ |  |
| [7](AdventOfCode2023/Day07.cs) | ⚫ |  |
| [8](AdventOfCode2023/Day08.cs) | ⚫ |  |
| [9](AdventOfCode2023/Day09.cs) | ⚫ |  |
| [10](AdventOfCode2023/Day10.cs) | ⚫ |  |
| [11](AdventOfCode2023/Day11.cs) | ⚫ |  |
| [12](AdventOfCode2023/Day12.cs) | ⚫ |  |
| [13](AdventOfCode2023/Day13.cs) | ⚫ |  |
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
