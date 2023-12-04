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
                .Select(v => Math.Pow(2, v - 1).Dump("pow"))
                .Sum()
                .ToString();
        }
    }

    [Day(ExpectedValue = "30")]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var cards = input.GetLines()
                .Select(l => l.Split(':'))
                .Select(l => new { card = l[0].Split(" ", StringSplitOptions.RemoveEmptyEntries)[1], nbWins = GetNbWins(l.Last()) })
                .ToDictionary(c => c.card, c => c.nbWins);

            return "AZE";
        }

        private int GetNbWins(string content)
        {
            var strs = content.Split('|');
            var win = strs[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .ToHashSet();
            var hand = strs[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            return hand.Count(win.Contains);
        }

    }
}
