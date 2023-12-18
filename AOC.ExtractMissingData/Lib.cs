namespace AOC.ExtractMissingData;

public class Lib
{
    public async Task Test2Async()
    {
        Console.WriteLine("Test2");
    }

    public Task TestAsync()
    {
        Console.WriteLine("Test");
        return Task.CompletedTask;
    }
}