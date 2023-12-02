namespace AdventOfCode.Lib;

[AttributeUsage(AttributeTargets.Class)]
public class DayAttribute : Attribute
{
    public string? ExpectedValue { get; set; }

    public string SampleInput { get; set; } = "testInput.txt";
}