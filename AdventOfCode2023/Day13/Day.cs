using AdventOfCode.Lib;

namespace AdventOfCode2023.Day13
{
    public class PuzzleBase
    {
    }

    [Day(ExpectedValue = "405")]
    public class Puzzle1 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.Split(Environment.NewLine)
                .Split(line => line.IsNullOrEmpty())
                .Select(GetColumnsAndLines)
                .Select(FindPattern)
                .Sum()
                .ToString();
        }

        private long FindPattern((List<long> columns, List<long> rows) input)
        {
            if (TryFindPattern(input.columns, out var result))
                return result;
            if (TryFindPattern(input.rows, out result))
                return result * 100;
            throw new Exception("No pattern found");
        }

        private bool TryFindPattern(List<long> values, out int index)
        {
            index = -1;
            for (int i = 0; i < values.Count - 1; i++)
            {
                int nbIteration = Math.Min(i + 1, values.Count - i - 1);
                bool isPattern = true;
                for (int j = 0; j < nbIteration; j++)
                {
                    if (values[i - j] == values[i + j + 1])
                    {
                        index = i + 1;
                    }
                    else
                    {
                        isPattern = false;
                        break;
                    }
                }

                if (isPattern)
                {
                    return true;
                }
            }

            return false;
        }

        public static (List<long> columns, List<long> rows) GetColumnsAndLines(IEnumerable<string> linesEnumerable)
        {
            var lines = linesEnumerable.Select(l => l.Replace('.', '0').Replace('#', '1').ToArray()).ToArray();
            var columns = new List<long>();
            for (int i = 0; i < lines[0].Length; i++)
            {
                columns.Add(Convert.ToInt64(string.Concat(lines.Select(l => l[i])), 2));
            }
            
            var rows = lines.Select(l => Convert.ToInt64(string.Concat(l).DumpLine("rows"), 2)).ToList();
            return (columns, rows);
        }
    }

    [Day(ExpectedValue = "400")]
    public class Puzzle2 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.Split(Environment.NewLine)
                .Split(line => line.IsNullOrEmpty())
                .Select(GetColumnsAndLines)
                .Select(FindPattern)
                .Sum()
                .ToString();
        }

        private long FindPattern((List<long> columns, List<long> rows) input)
        {
            var result = 0;
            bool hasBoth = true;
            if (TryFindPattern(input.columns, input.rows.Count, out var resultColumns))
            {
                hasBoth = !hasBoth;
                result += resultColumns;
            }

            if (TryFindPattern(input.rows, input.columns.Count, out var resultRows))
            {
                hasBoth = !hasBoth;
                result += resultRows * 100;
            }

            if (hasBoth)
                throw new Exception("No pattern found");
            return result;
        }

        private bool TryFindPattern(List<long> values, int sizeInt, out int index)
        {
            index = -1;
            HashSet<long> smudges = Enumerable.Range(0, sizeInt).Select(i => (long)(2 << i)).Prepend(1).ToHashSet();
            for (int i = 0; i < values.Count - 1; i++)
            {
                int nbIteration = Math.Min(i + 1, values.Count - i - 1);
                bool isSmudgeUsed = false;
                bool isPattern = true;
                for (int j = 0; j < nbIteration; j++)
                {
                    if (values[i - j] == values[i + j + 1])
                    {
                        index = i + 1;
                    }
                    else if (smudges.Contains(values[i - j] ^ values[i + j + 1]))
                    {
                        if (isSmudgeUsed)
                        {
                            isPattern = false;
                            break;
                        }
                        isSmudgeUsed = true;
                        index = i + 1;
                    }
                    else
                    {
                        isPattern = false;
                        break;
                    }
                }

                if (isPattern && isSmudgeUsed)
                {
                    return true;
                }
            }

            return false;
        }

        public static (List<long> columns, List<long> rows) GetColumnsAndLines(IEnumerable<string> linesEnumerable)
        {
            var lines = linesEnumerable.Select(l => l.Replace('.', '0').Replace('#', '1').ToArray()).ToArray();
            var columns = new List<long>();
            for (int i = 0; i < lines[0].Length; i++)
            {
                columns.Add(Convert.ToInt64(string.Concat(lines.Select(l => l[i])), 2));
            }

            var rows = lines.Select(l => Convert.ToInt64(string.Concat(l).DumpLine("rows"), 2)).ToList();
            return (columns, rows);
        }

    }
}
