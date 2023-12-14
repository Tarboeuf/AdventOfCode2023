using AdventOfCode.Lib;

namespace AdventOfCode2023.Day6
{

    [Day(ExpectedValue = "288")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var races = GetRaces(input);
            return races.Select(r => GetDifferentWaysToWin(r.time, r.distance).Count().DumpLine(">"))
                .Multiply().ToString();
        }

        private IEnumerable<int> GetDifferentWaysToWin(int time, int distance)
        {
            for (var i = 0; i < time; i++)
            {
                var newDistance = i * (time - i);
                if(newDistance.Dump("") > distance)
                    yield return i;
            }
        }

        private static List<(int time, int distance)> GetRaces(string input)
        {
            var lines = input.GetLines().ToList();
            var times = lines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var distances = lines[1].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            var races = new List<(int time, int distance)>();
            foreach (var time in times)
            {
                races.Add((time, distances[times.IndexOf(time)]));
            }

            return races;
        }
    }

    [Day(ExpectedValue = "71503", ShouldLog = true)]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var race = GetRaces(input);
            return GetDifferentWaysToWin(race.time, race.distance).ToString();
        }

        private long GetDifferentWaysToWin(long time, long distance)
        {
            long result = 0;
            for (long i = 0; i < time; i++)
            {
                var newDistance = i * (time - i);
                if (newDistance > distance)
                    result++;
            }
            return result;
        }

        private static (long time, long distance) GetRaces(string input)
        {
            var lines = input.GetLines().ToList();
            var time = lines[0].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", "").ToLong();
            var distance= lines[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", "").ToLong();
            
            return (time, distance);
        }
    }
}
    