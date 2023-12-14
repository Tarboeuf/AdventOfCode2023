using AdventOfCode2023.Day12;
using NUnit.Framework;
using System.Collections.Generic;
using AdventOfCode.Lib;

namespace AOC.Tests.Day12;

[TestFixture]
public class DayTests
{
    [TestCaseSource(nameof(CasesOne))]
    public long GetPossibleArrangementsTest(string stream, int[] condition)
    {
        return PuzzleBase.GetPossibleArrangements(stream, condition.ToList());
    }

    [TestCaseSource(nameof(CasesOne))]
    public long DpSolverTest(string stream, int[] condition)
    {
        return DpSolver.Compute(stream, condition.ToList());
    }


    public static IEnumerable<TestCaseData> CasesFive
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

    public static IEnumerable<TestCaseData> CasesOne
    {
        get
        {
            yield return new TestCaseData("?.??#?????#???.??", new[] { 8, 2 }).Returns(3);
            yield return new TestCaseData("??#?????#???.??", new[] { 8, 2 }).Returns(3);
            yield return new TestCaseData("?#?????#???.??", new[] { 8, 2 }).Returns(3);
            yield return new TestCaseData("?#??#???.??", new[] { 5, 2 }).Returns(3);
            yield return new TestCaseData("?##??.?", new[] { 3, 1 }).Returns(3);
            yield return new TestCaseData("?#??.?", new[] { 2, 1 }).Returns(3);
            yield return new TestCaseData("?.?", new[] { 1 }).Returns(2);

            yield return new TestCaseData("?#?.?", new[] { 2, 1 }).Returns(2);
            yield return new TestCaseData("?#??", new[] { 2 }).Returns(2);
            yield return new TestCaseData("#", new[] { 1 }).Returns(1);
            yield return new TestCaseData("##.#", new[] { 2, 1 }).Returns(1);
        }
    }
}