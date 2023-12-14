using AdventOfCode2023.Day12;
using NUnit.Framework;
using System.Collections.Generic;

namespace AOC.Tests.Day12;

[TestFixture]
public class DayTests
{
    [TestCaseSource(nameof(Cases))]
    public long GetPossibleArrangementsTest(string stream, List<int> condition)
    {
        return Puzzle1.GetPossibleArrangements(stream, condition);
    }

    [TestCaseSource(nameof(RemoveMatchingPartsCases))]
    public (string stream, List<int> condition) RemoveMatchingPartsTest(string stream, List<int> condition)
    {
        var (newStream, newCondition) = Puzzle1.RemoveMatchingParts(stream, condition);
        return Puzzle1.RemoveMatchingParts(newStream, newCondition);
    }

    [TestCase("???", 3, ExpectedResult = 1)]
    [TestCase("???", 1, ExpectedResult = 3)]
    [TestCase("???", 2, ExpectedResult = 2)]
    [TestCase("??", 1, ExpectedResult = 2)]
    [TestCase("?##", 2, ExpectedResult = 1)]
    [TestCase("??##", 2, ExpectedResult = 1)]
    [TestCase("?#?", 2, ExpectedResult = 2)]
    [TestCase("?#?", 2, ExpectedResult = 2)]
    [TestCase("#??", 2, ExpectedResult = 1)]
    [TestCase("??#?", 2, ExpectedResult = 2)]
    [TestCase("?#??", 2, ExpectedResult = 2)]
    [TestCase("?##?", 3, ExpectedResult = 2)]
    [TestCase("?###?", 3, ExpectedResult = 1)]
    [TestCase("?###?", 4, ExpectedResult = 2)]
    [TestCase("??##??", 4, ExpectedResult = 3)]
    [TestCase("?#?????", 4, ExpectedResult = 2)]
    [TestCase("?????#?????", 4, ExpectedResult = 4)]
    [TestCase("??????#?", 2, ExpectedResult = 2)]
    public int GetMatchesTest(string stream, int nbBreaks)
    {
        return Puzzle1.GetMatches(stream, nbBreaks);
    }


    public static IEnumerable<TestCaseData> Cases
    {
        get
        {
            yield return new TestCaseData("???.###", new List<int> { 1, 1, 3 }).Returns(1);
            yield return new TestCaseData(".??..??...?##.", new List<int> { 1, 1, 3 }).Returns(4);
            yield return new TestCaseData("?#?#?#?#?#?#?#?", new List<int> { 1, 3, 1, 6 }).Returns(1);
            yield return new TestCaseData("????.#...#...", new List<int> { 4, 1, 1 }).Returns(1);
            yield return new TestCaseData("????.######..#####.", new List<int> { 1, 6, 5 }).Returns(4);
            yield return new TestCaseData("?###????????", new List<int> { 3, 2, 1 }).Returns(10);
            yield return new TestCaseData("?##????????.?###?#.", new List<int> { 8, 1, 4, 1 }).Returns(3);
            yield return new TestCaseData("?##???.?###?#.", new List<int> { 3, 1, 4, 1 }).Returns(3);
        }
    }
    public static IEnumerable<TestCaseData> RemoveMatchingPartsCases
    {
        get
        {
            yield return new TestCaseData("???.###", new List<int> { 1, 1, 3 }).Returns(("???", new List<int> { 1, 1 }));
            yield return new TestCaseData("????.######..#####.", new List<int> { 1, 1, 3 }).Returns(("????", new List<int> { 1 }));
            yield return new TestCaseData("?#?#?#?#?#?#?#?", new List<int> { 1, 3, 1, 6 }).Returns(("?#?#?#?#?#?#?#?", new List<int> { 1, 3, 1, 6 }));
            yield return new TestCaseData("????.#...#...", new List<int> { 4, 1, 1 }).Returns(("????", new List<int> { 4 }));
            yield return new TestCaseData("?##???.?###?#.", new List<int> { 3, 1, 4, 1 }).Returns(("?##???", new List<int> { 3, 1 }));
        }
    }
}