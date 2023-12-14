using AdventOfCode2023.Day12;
using NUnit.Framework;
using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AOC.Tests.Day12;

[TestFixture]
public class DayTests
{
    [TestCaseSource(nameof(Cases))]
    public long DpTest(string stream, int[] condition)
    {
        return Puzzle2.Dp(stream.Repeat(5, "?"), condition.Repeat(5).ToArray());
    }
    

    public static IEnumerable<TestCaseData> Cases
    {
        get
        {
            yield return new TestCaseData("???.###", new[] { 1, 1, 3 }).Returns(1);
            yield return new TestCaseData("??.#???.#?", new[] { 1, 1 }).Returns(1);
            yield return new TestCaseData(".??..??...?##.", new [] { 1, 1, 3 }).Returns(16384);
            yield return new TestCaseData("?#?#?#?#?#?#?#?", new [] { 1, 3, 1, 6 }).Returns(1);
            yield return new TestCaseData("????.#...#...", new [] { 4, 1, 1 }).Returns(16);
            yield return new TestCaseData("????.######..#####.", new [] { 1, 6, 5 }).Returns(2500);
            yield return new TestCaseData("?###????????", new [] { 3, 2, 1 }).Returns(506250);
        }
    }
}