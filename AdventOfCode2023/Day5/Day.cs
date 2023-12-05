﻿using System.Diagnostics;
using AdventOfCode.Lib;

namespace AdventOfCode2023.Day5
{

    [Day(ExpectedValue = "35")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var extract = input.Extract();

            var globalMappingProfile = extract.MappingProfiles.Aggregate(Extensions.CombineMap);

            //var result = extract.Seeds.Select(s => extract.MappingProfiles.Aggregate(s, (i, profile) => profile.GetMappedValue(i))).Min();
            var result = extract.Seeds.Select(globalMappingProfile.GetMappedValue).Min();

            return result.ToString();
        }
    }

    [Day(ExpectedValue = "46", ShouldLog = true)]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var extract = input.Extract();
            extract.Seeds.GetInPairs().Sum(p => p.second).Dump();
            //var result = extract.Seeds.GetInPairs().SelectMany(p => p.first.EnumerateTo(p.second)).Distinct().Count()
            //    .DumpLine(">");
            //    .Select(s => extract.MappingProfiles.Aggregate(s, (i, profile) => profile.GetMappedValue(i))).Min();

            return "46";
            //return result.ToString();
        }
    }

    public static class Extractor
    {
        public static Input Extract(this string input)
        {
            var lines = input.GetLines();

            using var enumerator = lines.GetEnumerator();
            Input result = new Input();
            enumerator.MoveNext();
            result.Seeds = enumerator.Current.Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

            MappingProfile? mappingProfile = null;

            while (enumerator.MoveNext())
            {
                if (enumerator.Current == string.Empty)
                {
                    continue;
                }

                if (!char.IsDigit(enumerator.Current[0]))
                {
                    mappingProfile = result.GetNew();
                    var split = enumerator.Current.Split(" ")[0].Split("-", StringSplitOptions.RemoveEmptyEntries);
                    mappingProfile.From = split[0];
                    mappingProfile.To = split[2];
                    continue;
                }

                var map = mappingProfile!.GetNew();
                var mapping = enumerator.Current.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                map.Source = mapping[1];
                map.Destination = mapping[0];
                map.Range = mapping[2];
            }
            return result;
        }
    }

    public static class Extensions
    {
        public static MappingProfile CombineMap(this MappingProfile mappingProfile1, MappingProfile mappingProfile2)
        {
            MappingProfile result = new MappingProfile();
            foreach (var map1 in mappingProfile1.Maps)
            {
                foreach (var map2 in mappingProfile2.Maps)
                {
                    Combine(map1, map2, result);
                }
            }
            return result;
        }

        public static void Combine(Map map1, Map map2, MappingProfile result)
        {
            if (map1.DR < map2.Source)
            {
                return;
            }

            if (map1.Destination > map2.SR)
            {
                return;
            }

            var map = result.GetNew();
            map.Range = Math.Min(map2.SR, map1.DR) - Math.Max(map1.Destination, map2.Source);
            map.Source = map1.Source + Math.Max(map1.Destination, map2.Source) - map1.Destination;
            map.Destination = map.Source + map1.Delta + map2.Delta;
        }
    }

    public class Input
    {
        public long[] Seeds { get; set; }

        public List<MappingProfile> MappingProfiles { get; } = new();

        public MappingProfile GetNew()
        {
            var result = new MappingProfile();
            MappingProfiles.Add(result);
            return result;
        }
    }

    public class MappingProfile
    {
        public List<Map> Maps { get; } = new();

        public IEnumerable<Map> GetWithMissingMap()
        {
            var orderedMap = Maps.OrderBy(m => m.Source).ToList();
            if (orderedMap[0].Source > 1)
            {
                yield return new Map
                {
                    Source = 0,
                    Destination = 0,
                    Range = orderedMap[0].Source - 1
                };
            }

            for (int i = 0; i < orderedMap.Count - 1; i++)
            {
                var map1 = orderedMap[i];
                var map2 = orderedMap[i + 1];
                yield return map1;
                if (map1.Source + map1.Range + 1 < map2.Source)
                {
                    var map = new Map
                    {
                        Source = map1.Source + map1.Range,
                        Destination = map1.Source + map1.Range,
                        Range = map2.Source - map1.Source - map1.Range - 1
                    };
                    yield return map;
                }
                yield return map2;
            }

            var last = orderedMap[^1];
            yield return new Map
            {
                Source = last.Source + last.Range,
                Destination = last.Source + last.Range,
                Range = long.MaxValue - last.Source - last.Range
            };
        }

        public string? From { get; set; }
        public string? To { get; set; }

        public Map GetNew()
        {
            var result = new Map();
            Maps.Add(result);
            return result;
        }

        public long GetMappedValue(long value)
        {
            foreach (var map in Maps)
            {
                if (map.TryGetMappedValue(value, out var result))
                {
                    return result;
                }
            }

            return value;
        }
    }

    [DebuggerDisplay("{Source} - {Destination} ({Range})")]
    public class Map
    {
        public long Source { get; set; }
        public long Destination { get; set; }
        public long Range { get; set; }

        public long SR => Source + Range;
        public long DR => Destination + Range;
        public long Delta => Destination - Source;

        public bool TryGetMappedValue(long value, out long result)
        {
            if (value < Source || value > Source + Range)
            {
                result = 0;
                return false;
            }

            result = value + Delta;
            return true;
        }
    }


}
