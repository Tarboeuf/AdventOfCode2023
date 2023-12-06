using System.Linq;
using System.Security.AccessControl;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Day2
{
    [Day(ExpectedValue = "2286")]
    public class Day02Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var games = input.GetLines().Select(l =>
            {
                var strs = l.Split(":", StringSplitOptions.RemoveEmptyEntries);

                return new Game()
                {
                    Id = int.Parse(strs[0].Replace("Game ", "")),
                    Sets = strs[1]
                        .Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries))
                            .ToDictionary(
                                s => s[1],
                                s => int.Parse(s[0]))).ToList(),

                };
            });

            return games.Select(g =>
                {
                    var kvps = g.Sets.SelectMany(s => s);
                    return new { g.Id, dico = kvps.GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Max(t => t.Value)).DumpLine("Dico") };
                })
                .Select(d => d.dico.Values.Multiply())
                .Sum().ToString();
        }
    }

    [Day(ExpectedValue = "8")]
    public class Day02Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var games = input.GetLines().Select(l =>
            {
                var strs = l.Split(":", StringSplitOptions.RemoveEmptyEntries);

                return new Game()
                {
                    Id = int.Parse(strs[0].Replace("Game ", "")),
                    Sets = strs[1]
                        .Split(";", StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => v.Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries))
                            .ToDictionary(
                                s => s[1],
                                s => int.Parse(s[0]))).ToList(),
                        
                };
            });

            return games.Select(g =>
                {
                    var kvps = g.Sets.SelectMany(s => s);
                    return new {g.Id, dico = kvps.GroupBy(kvp => kvp.Key).ToDictionary(g => g.Key, g => g.Max(t => t.Value)).DumpLine("Dico")};
                })
                .Where(d => (!d.dico.ContainsKey("red") || d.dico["red"] <= 12) &&
                            (!d.dico.ContainsKey("green") || d.dico["green"] <= 13) &&
                            (!d.dico.ContainsKey("blue") || d.dico["blue"] <= 14))
                .Sum(g => g.Id.DumpLine("Id")).ToString();

        }
    }

    public class Game
    {
        public int Id { get; set; }
        public List<Dictionary<string, int>> Sets { get; set; }
    }
}
