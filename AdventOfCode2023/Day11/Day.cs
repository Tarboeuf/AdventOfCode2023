using AdventOfCode.Lib;

namespace AdventOfCode2023.Day11
{
    public class PuzzleBase
    {
    }

    [Day(ExpectedValue = "374")]
    public class Puzzle1 : PuzzleBase,  IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var table = input.GetLines().ToArray();
            var values = new List<(int x, int y)>();
            for (var y = 0; y < table.Length; y++)
            {
                for (var x = 0; x < table[y].Length; x++)
                {
                    if (table[y][x] == '#')
                    {
                        values.Add((x, y));
                    }
                }
            }

            var maxX = values.Max(v => v.x);
            var maxY = values.Max(v => v.y);
            var allX = values.Select(v => v.x).Distinct().OrderBy(v => v).ToArray();
            var allY = values.Select(v => v.y).Distinct().OrderBy(v => v).ToArray();
            var emptyX = Enumerable.Range(0, maxX + 1).Except(allX).OrderByDescending(i => i).ToArray();
            var emptyY = Enumerable.Range(0, maxY + 1).Except(allY).OrderByDescending(i => i).ToArray();

            var valuesTransformed = new HashSet<(int x, int y)>();

            for (var i = 0; i < values.Count; i++)
            {
                var value = values[i];
                valuesTransformed.Add((value.x + emptyX.Count(x => x < value.x),
                    value.y + emptyY.Count(y => y < value.y)));
            }

            maxX = valuesTransformed.Max(v => v.x);
            maxY = valuesTransformed.Max(v => v.y);
            if (!isRealCase)
            {
                for (var y = 0; y <= maxY; y++)
                {
                    Console.WriteLine();
                    for (var x = 0; x <= maxX; x++)
                    {
                        Console.Write(valuesTransformed.Contains((x, y)) ? '#' : '.');
                    }
                }
            }

            return valuesTransformed.GetAllPairs().DumpLine().Select(p => Math.Abs(p.Item1.x - p.Item2.x) + Math.Abs(p.Item1.y - p.Item2.y)).Sum().ToString();
        }

    }

    [Day(ExpectedValue = "8410")]
    public class Puzzle2 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var table = input.GetLines().ToArray();
            var values = new List<(int x, int y)>();
            for (var y = 0; y < table.Length; y++)
            {
                for (var x = 0; x < table[y].Length; x++)
                {
                    if (table[y][x] == '#')
                    {
                        values.Add((x, y));
                    }
                }
            }

            var maxX = values.Max(v => v.x);
            var maxY = values.Max(v => v.y);
            var allX = values.Select(v => v.x).Distinct().OrderBy(v => v).ToArray();
            var allY = values.Select(v => v.y).Distinct().OrderBy(v => v).ToArray();
            var emptyX = Enumerable.Range(0, maxX + 1).Except(allX).OrderByDescending(i => i).ToArray();
            var emptyY = Enumerable.Range(0, maxY + 1).Except(allY).OrderByDescending(i => i).ToArray();

            var valuesTransformed = new HashSet<(long x, long y)>();
            long multiplier = (isRealCase ? 1000000 : 100) - 1;

            foreach (var value in values)
            {
                valuesTransformed.Add((value.x + emptyX.Count(x => x < value.x) * multiplier,
                    value.y + emptyY.Count(y => y < value.y) * multiplier));
            }

            return valuesTransformed.GetAllPairs().Select(p => Math.Abs(p.Item1.x - p.Item2.x) + Math.Abs(p.Item1.y - p.Item2.y)).Sum().ToString();
        }
    }
}
