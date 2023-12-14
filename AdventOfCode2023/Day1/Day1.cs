using AdventOfCode.Lib;

namespace AoC.Day1
{
    [Day(ExpectedValue = "281", SampleInput = "testInput2.txt")]
    public class Day01Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines().Select(s => int.Parse(string.Concat(GetFirstNumber(s), GetLastNumber(s))).DumpLine(s)).Sum().ToString();
        }

        private char GetFirstNumber(string line)
        {
            for (var i = 0; i < line.Length; i++)
            {
                if (char.IsDigit(line[i])) return line[i];
                for (var j = 0; j < _spelledDigits.Length; j++)
                {
                    if (line[i..].StartsWith(_spelledDigits[j]))
                    {
                        return (char)('0' + j);
                    }
                }
            }

            throw new IndexOutOfRangeException(line);
        }
        private char GetLastNumber(string line)
        {
            for (var i = line.Length - 1; i >= 0; i--)
            {
                if (char.IsDigit(line[i])) return line[i];
                var test = line[..^(line.Length - i - 1)].DumpLine("..î");

                for (var j = 0; j < _spelledDigits.Length; j++)
                {
                    if (test.EndsWith(_spelledDigits[j]))
                    {
                        return (char)('0' + j);
                    }
                }
            }

            throw new IndexOutOfRangeException(line);
        }

        private readonly string[] _spelledDigits = Enumerable.Range(0, 10).Select(SpellDigit).ToArray();

        static string SpellDigit(int digit)
        {
            switch (digit)
            {
                case 0:
                    return "zero";
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    return "invalid digit";
            }
        }
    }

    [Day(ExpectedValue = "142")]
    public class Day01Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines().Select(s => int.Parse(string.Concat(s.First(char.IsDigit), s.Last(char.IsDigit)))).Sum().ToString();
        }
    }
}
