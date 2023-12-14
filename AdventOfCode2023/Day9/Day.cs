using AdventOfCode.Lib;

namespace AdventOfCode2023.Day9
{

    [Day(ExpectedValue = "114")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(v => v.ToInt()))
                .Select(GetNextValue)
                .Sum().ToString();
        }

        private int GetNextValue(IEnumerable<int> values)
        {
            var currentValues = values.ToList();
            var lastValues = new List<int>();
            while (true)
            {
                lastValues.Add(currentValues.Last());
                currentValues = currentValues.Zip(currentValues.Skip(1))
                    .Select(v => v.Second - v.First)
                    .ToList();
                if (currentValues.Distinct().Count() == 1)
                {
                    lastValues.Add(currentValues.Last());
                    return lastValues.Sum();
                }
            }
        }
    }

    [Day(ExpectedValue = "2")]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(l => l.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(v => v.ToInt()))
                .Select(GetPreviousValue)
                .Sum().ToString();
        }

        private int GetPreviousValue(IEnumerable<int> values)
        {
            var currentValues = values.ToList();
            var firstValues = new List<int>();
            while (true)
            {
                firstValues.Add(currentValues.First());
                currentValues = currentValues.Zip(currentValues.Skip(1))
                    .Select(v => v.Second - v.First)
                    .ToList();
                if (currentValues.Distinct().Count() == 1)
                {
                    firstValues.Reverse();

                    var current  = currentValues.First();
                    foreach (var firstValue in firstValues)
                    {
                        current = firstValue - current;
                    }

                    return current;
                }
            }
        }
    }
}
