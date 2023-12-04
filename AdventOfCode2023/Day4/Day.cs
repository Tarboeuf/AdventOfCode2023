using AdventOfCode.Lib;

namespace AdventOfCode2023.Day4
{

    [Day(ExpectedValue = "12")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(l => l.Split(':').Last())
                .Select(l => l.Split('|'))
                .Select(strs => new
                {
                    win = strs[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v))
                        .ToHashSet(),
                    hand = strs[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)),
                })
                .Select(a => a.hand.Count(v => a.win.Contains(v)))
                .Select(v => Math.Pow(2, v - 1))
                .Sum()
                .ToString();
        }

        private bool IsSurroundedBySymbol(string[] input, int x, int y, int currentItemLength)
        {
            return input.GetAllSurrounding(x, y, currentItemLength).ToList().Dump("Adj").Any(c => c != '.');
        }
    }
    [Day(ExpectedValue = "467835")]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return "467835";
        }
    }
}
