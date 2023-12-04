using AdventOfCode.Lib;

namespace AdventOfCode2023.Day3
{

    [Day(ExpectedValue = "13")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var lines = input.GetLines().ToArray();
            var width = lines.First().Length;
            var height = lines.Count();

            List<int> values = new List<int>();
            for (int y = 0; y < height; y++)
            {
                var line = lines[y];
                string? currentItem = null;
                for (int x = 0; x < width; x++)
                {
                    var c = line[x];
                    if (!char.IsDigit(c))
                    {
                        if (currentItem != null && IsSurroundedBySymbol(lines, x - currentItem.Dump("item").Length, y, currentItem.Length))
                        {
                            values.Add(int.Parse(currentItem));
                        }
                        currentItem = null;
                    }
                    else
                    {
                        currentItem += c;
                    }
                }
                if (currentItem != null && IsSurroundedBySymbol(lines, width - currentItem.Dump("item").Length, y, currentItem.Length))
                {
                    values.Add(int.Parse(currentItem));
                }
            }
            return values.Sum().ToString();
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
            var lines = input.GetLines().ToArray();
            var width = lines.First().Length;
            var height = lines.Count();

            long result = 0;
            for (int y = 0; y < height; y++)
            {
                var line = lines[y];
                
                for (int x = 0; x < width; x++)
                {
                    var c = line[x];
                    if (c == '*')
                    {
                        var numbers = GetAllSurroundingNumbers(lines, x, y);
                        if (numbers.Count == 2)
                        {
                            result += numbers[0] * numbers[1];
                        }
                    }
                }
            }
            return result.ToString();
        }

        private List<int> GetAllSurroundingNumbers(string[] input, int x, int y)
        {
            return input.GetAllSurroundingNumbers(x, y).Distinct().Select(n => n.Number).ToList();
        }

        private bool IsSurroundedBySymbol(string[] input, int x, int y, int currentItemLength)
        {
            return input.GetAllSurrounding(x, y, currentItemLength).ToList().Dump("Adj").Any(c => c != '.');
        }
    }
}
