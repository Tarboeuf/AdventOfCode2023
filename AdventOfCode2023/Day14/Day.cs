using AdventOfCode.Lib;
using System.Linq;

namespace AdventOfCode2023.Day14
{
    public class PuzzleBase
    {
    }

    [Day(ExpectedValue = "136")]
    public class Puzzle1 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return 
                input.GetColumns()
                    .Select((c, i) =>
                    {
                        i.DumpLine("Col");
                        return new {tilt = Tilt(c.GetIndexesOf('O'), c.GetIndexesOf('#')), size = c.Count};
                    })
                    .Select(i => i.tilt.Sum(t => i.size - t.DumpLine()) )
                    .Sum()
                    .ToString();
        }
        
        public IEnumerable<int> Tilt(IEnumerable<int> roundedRocks, IEnumerable<int> obstacles)
        {
            using var rocks = roundedRocks.OrderBy(r => r).GetEnumerator();
            using var obs = obstacles.GetEnumerator();
            var currentObstacle = -1;
            if (!rocks.MoveNext())
            {
                yield break;
            }
            int currentRock = rocks.Current;
            int index;
            while (obs.MoveNext())
            {
                index = currentObstacle + 1;
                while (currentRock < obs.Current)
                {
                    yield return index;
                    index++;
                    if(rocks.MoveNext())
                        currentRock = rocks.Current;
                    else
                        yield break;
                }
                currentObstacle = obs.Current;
            }
            index = currentObstacle + 1;
            yield return index++;
            while (rocks.MoveNext())
            {
                yield return index++;
            }
        }
    }

    [Day(ExpectedValue = "64", ShouldLog = true)]
    public class Puzzle2 : PuzzleBase, IDay
    {
        private const long NbCycle = 1_000_000_000;

        private Dictionary<int, List<int>> _northObstacles = new();
        private Dictionary<int, List<int>> _southObstacles = new();
        private Dictionary<int, List<int>> _eastObstacles = new();
        private Dictionary<int, List<int>> _westObstacles = new();

        private Dictionary<List<(int x, int y)>, long> _memoization = new(new ListComparer<(int x, int y)>(tuple => (int)((long)tuple.x * tuple.y)));

        private int _width;
        private int _height;

        public string GetPuzzle(string input, bool isRealCase)
        {
            var lines = input.GetLines().ToArray();
            _memoization = new(new ListComparer<(int x, int y)>(tuple => (int) ((long) tuple.x * tuple.y)));
            _width = lines[0].Length;
            _height = lines.Length;
            List<(int x, int y)> obstacles = lines.SelectMany((l, y) => l.GetIndexesOf('#').Select(x => (x, y))).ToList();
            IEnumerable<(int x, int y)> rocks = lines.SelectMany((l, y) => l.GetIndexesOf('O').Select(x => (x, y)));

            _northObstacles = obstacles.GroupBy(o => o.x).ToDictionary(g => g.Key, g => g.Select(c => c.y).OrderBy(r => r).ToList());
            _southObstacles = obstacles.GroupBy(o => o.x).ToDictionary(g => g.Key, g => g.Select(c => c.y).OrderByDescending(r => r).ToList());
            _westObstacles = obstacles.GroupBy(o => o.y).ToDictionary(g => g.Key, g => g.Select(c => c.x).OrderBy(r => r).ToList());
            _eastObstacles = obstacles.GroupBy(o => o.y).ToDictionary(g => g.Key, g => g.Select(c => c.x).OrderByDescending(r => r).ToList());

            var currentCycle = rocks.ToList();

            for (long i = 0; i < NbCycle; i++)
            {
                currentCycle = CurrentCycle(currentCycle, ref i); 
                currentCycle.ExportImage(obstacles, _width, _height, $"C:\\Temp\\{i}.png");
            }
            
            _memoization.ForEach(c => c.Key.Select(r => _height - r.y)
                .Sum().DumpLine(c.Value.ToString()));

            return
                currentCycle
                    .Select(r => _height - r.y)
                    .Sum()
                    .ToString();
        }

        private List<(int x, int y)> CurrentCycle(List<(int x, int y)> currentCycle, ref long i)
        {
            if (_memoization.ContainsKey(currentCycle) && i == _memoization.Count)
            {
                int previousOccurence = (int)_memoization[currentCycle];
                var diff = i.Dump("i") - previousOccurence.Dump("value");
                i = ((NbCycle) / diff) * diff + previousOccurence;
            }

            var rocks = North(currentCycle);
            rocks = West(rocks);
            rocks = South(rocks);
            rocks = East(rocks);
            var cycle = rocks.OrderBy(c => c.x).ThenBy(c => c.y).ToList();
            _memoization.TryAdd(currentCycle, i);

            currentCycle = cycle;
            return currentCycle;
        }

        private IEnumerable<(int x, int y)> North(IEnumerable<(int x, int y)> rocks)
        {
            var rocksDictionary = rocks.GroupBy(o => o.x).ToDictionary(g => g.Key, g => g.Select(c => c.y).OrderBy(r => r).ToList());
            for (int x = 0; x < _width; x++)
            {
                if (!rocksDictionary.ContainsKey(x))
                {
                    continue;
                }
                
                foreach (var newPos in TiltMin(rocksDictionary[x], _northObstacles.TryGetValue(x, out var obstacle) ? obstacle : Enumerable.Empty<int>()))
                {
                    yield return (x, newPos);
                }
            }
        }
        private IEnumerable<(int x, int y)> South(IEnumerable<(int x, int y)> rocks)
        {
            var rocksDictionary = rocks.GroupBy(o => o.x).ToDictionary(g => g.Key, g => g.Select(c => c.y).OrderByDescending(r => r).ToList());
            for (int x = 0; x < _width; x++)
            {
                if (!rocksDictionary.ContainsKey(x))
                {
                    continue;
                }

                foreach (var newPos in TiltMax(rocksDictionary[x], _southObstacles.TryGetValue(x, out var obstacle) ? obstacle : Enumerable.Empty<int>(), _height))
                {
                    yield return (x, newPos);
                }
            }
        }

        private IEnumerable<(int x, int y)> East(IEnumerable<(int x, int y)> rocks)
        {
            var rocksDictionary = rocks.GroupBy(o => o.y).ToDictionary(g => g.Key, g => g.Select(c => c.x).OrderByDescending(r => r).ToList());
            for (int y = 0; y < _height; y++)
            {
                if (!rocksDictionary.ContainsKey(y))
                {
                    continue;
                }

                foreach (var newPos in TiltMax(rocksDictionary[y], _eastObstacles.TryGetValue(y, out var obstacle) ? obstacle : Enumerable.Empty<int>(), _width))
                {
                    yield return (newPos, y);
                }
            }
        }

        private IEnumerable<(int x, int y)> West(IEnumerable<(int x, int y)> rocks)
        {
            var rocksDictionary = rocks.GroupBy(o => o.y).ToDictionary(g => g.Key, g => g.Select(c => c.x).OrderBy(r => r).ToList());
            for (int y = 0; y < _height; y++)
            {
                if (!rocksDictionary.ContainsKey(y))
                {
                    continue;
                }

                foreach (var newPos in TiltMin(rocksDictionary[y], _westObstacles.TryGetValue(y, out var obstacle) ? obstacle : Enumerable.Empty<int>()))
                {
                    yield return (newPos, y);
                }
            }
        }

        private IEnumerable<int> TiltMin(IEnumerable<int> roundedRocks, IEnumerable<int> obstacles)
        {
            using var rocks = roundedRocks.GetEnumerator();
            using var obs = obstacles.GetEnumerator();
            var currentObstacle = -1;
            if (!rocks.MoveNext())
            {
                yield break;
            }
            int currentRock = rocks.Current;
            int index;
            while (obs.MoveNext())
            {
                index = currentObstacle + 1;
                while (currentRock < obs.Current)
                {
                    yield return index;
                    index++;
                    if (rocks.MoveNext())
                        currentRock = rocks.Current;
                    else
                        yield break;
                }
                currentObstacle = obs.Current;
            }
            index = currentObstacle + 1;
            yield return index++;
            while (rocks.MoveNext())
            {
                yield return index++;
            }
        }
        private IEnumerable<int> TiltMax(IEnumerable<int> roundedRocks, IEnumerable<int> obstacles, int size)
        {
            using var rocks = roundedRocks.GetEnumerator();
            using var obs = obstacles.GetEnumerator();
            var currentObstacle = size;
            if (!rocks.MoveNext())
            {
                yield break;
            }
            int currentRock = rocks.Current;
            int index;
            while (obs.MoveNext())
            {
                index = currentObstacle - 1;
                while (currentRock > obs.Current)
                {
                    yield return index;
                    index--;
                    if (rocks.MoveNext())
                        currentRock = rocks.Current;
                    else
                        yield break;
                }
                currentObstacle = obs.Current;
            }
            index = currentObstacle - 1;
            yield return index--;
            while (rocks.MoveNext())
            {
                yield return index--;
            }
        }

    }
}
