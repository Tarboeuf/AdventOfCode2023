using AdventOfCode.Lib;

namespace AdventOfCode2023.Day4
{

    [Day(ExpectedValue = "13")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(l => l.Split(':').Last())
                .Select(l => l.Split('|'))
                .Select(strs => new
                {
                    win = strs[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                        .ToHashSet(),
                    hand = strs[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse),
                })
                .Select(a => a.hand.Count(a.win.Contains))
                .Where(v => v != 0)
                .Select(v => Math.Pow(2, v - 1).DumpLine("pow"))
                .Sum()
                .ToString();
        }
    }

    [Day(ExpectedValue = "30")]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var wins = input.GetLines()
                .Select(l => l.Split(':').Last())
                .Select(l => l.Split('|'))
                .Select(strs => new
                {
                    win = strs[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                        .ToHashSet(),
                    hand = strs[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse),
                })
                .Select(a => a.hand.Count(a.win.Contains))
                .ToArray();
            var occurrences = Enumerable.Range(0, wins.Length).Select(e => 1).ToArray();
            for (var i = 0; i < occurrences.Length - 1; i++)
            {
                for (var j = 0; j < wins[i]; j++)
                {
                    occurrences[i + 1 + j] += occurrences[i];
                }
            }

            return occurrences
                .Sum()
                .ToString();
        }
    }
}
