using System.Drawing.Printing;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Day12
{
    public class PuzzleBase
    {
        public static long GetPossibleArrangements(string stream, List<int> condition)
        {
            var questionsIndex = stream.Select((c, i) => (c, i)).Where(t => t.c == '?').Select(t => t.i).ToList();

            var combinations = questionsIndex.GetPermutations(condition.Sum() - stream.Count(c => c == '#'));

            var sum = 0;
            foreach (var combination in combinations)
            {
                var newStream = stream.Select((c, i) => combination.Contains(i) ? '#' : (c == '?' ? '.' : c));
                var list = newStream.Split(c => c == '.').Select(e => e.Count()).Where(c => c != 0).ToList();
                if (list.SequenceEqual(condition))
                {
                    sum++;
                }
            }

            return sum;
        }
    }

    [Day(ExpectedValue = "21")]
    public class Puzzle1 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(line => line.Split(' '))
                .Select(split => new
                {
                    stream = split[0],
                    condition = split[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToInt())
                        .ToList()
                })
                //.Select(o => GetPossibleArrangements(o.stream, o.condition))
                .Select(o => DpSolver.Compute(o.stream, o.condition.ToList()))
                //.DumpLine()
                .Sum()
                .ToString();
        }
    }

    [Day(ExpectedValue = "525152")]
    public class Puzzle2 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(line => line.Split(' '))
                .Select(split => new
                {
                    stream = split[0].Repeat(5, "?"),
                    condition = split[1].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToInt()).ToList().Repeat(5)
                        .ToList()
                })
                .Select(o => DpSolver.Compute(o.stream, o.condition.ToList()))
                //.DumpLine()
                .Sum()
                .ToString();
        }

    }

}

public static class DpSolver
{
    static readonly Regex DoubleDot = new Regex(@"\.\.+", RegexOptions.Compiled);
    public static long Compute(string springs, List<int> conditions)
    {
        springs = DoubleDot.Replace(springs, ".");
        List<List<long>> dp = Enumerable.Range(0, conditions.Count).Select(_ => new List<long>()).ToList();

        // Base case: dp[0]
        var firstCondition = conditions[0];
        for (var j = 0; j <= springs.Length; j++)
        {
            if (j < springs.Length && springs[j] == '#')
            {
                dp[0].Add(0);
                continue;
            }

            var startIndex = j - firstCondition;

            if (startIndex < 0)
            {
                dp[0].Add(0);
                continue;
            }

            var count = springs.Substring(startIndex, j - startIndex).Count(ch => ch is '#' or '?');

            if (count == firstCondition && springs.Substring(0, startIndex).All(ch => ch is '.' or '?'))
            {
                dp[0].Add(1);
            }
            else
            {
                dp[0].Add(0);
            }
        }

        for (var i = 1; i < conditions.Count; i++)
        {
            for (var j = 0; j <= springs.Length; j++)
            {
                long dpResult = 0;

                for (var k = 0; k < j - 1; k++)
                {
                    if (j < springs.Length && springs[j] == '#')
                    {
                        continue;
                    }

                    var startIndex = j - conditions[i];
                    
                    if (startIndex <= k)
                    {
                        continue;
                    }

                    var count = springs.Substring(startIndex, j - startIndex).Count(ch => ch == '#' || ch == '?');

                    if (count == conditions[i] && springs.Substring(k, startIndex - k).All(ch => ch == '.' || ch == '?'))
                    {
                        dpResult += dp[i - 1][k];
                    }
                }

                dp[i].Add(dpResult);
            }
        }

        long answer = 0;

        for (var i = springs.Length; i >= 0; i--)
        {
            if (i < springs.Length && springs[i] == '#')
            {
                break;
            }
            

            answer += dp.Last()[i].DumpLine();
        }

        return answer;
    }
}
