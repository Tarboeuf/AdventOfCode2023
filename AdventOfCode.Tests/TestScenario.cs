using AdventOfCode.Lib;

namespace AOC.Tests;

public class TestScenario
{
    public TestScenario(IDay day, string expectedResult, string name, string samplePath, string realPath,
        bool attributeShouldLog)
    {
        Day = day;
        ExpectedResult = expectedResult;
        Name = name;
        SamplePath = samplePath;
        RealPath = realPath;
        AttributeShouldLog = attributeShouldLog;
    }

    public IDay Day { get; init; }
    public string SamplePath { get; init; }
    public string RealPath { get; init; }
    public bool AttributeShouldLog { get; }
    public string ExpectedResult { get; init; }
    public string Name { get; init; }

    public override string ToString()
    {
        return Name;
    }
}