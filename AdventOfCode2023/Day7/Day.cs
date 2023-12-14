using AdventOfCode.Lib;

namespace AdventOfCode2023.Day7
{

    [Day(ExpectedValue = "6440")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(c => new Hand(c))
                .OrderBy(h => h.Weight).ThenBy(h => h.Value)
                .Select((h, index) => h.Bid * (index + 1))
                .Sum().ToString();
        }
    }

    public enum HandValue
    {
        HighCard = 0,
        Pair = 1,
        TwoPair = 2,
        ThreeOfAKind = 3,
        FullHouse = 4,
        FourOfAKind = 5,
        FiveOfAKind = 6
    }

    public class Hand : HandBase
    {
        public Hand(string line) : base(line)
        {
        }

        protected override int GetValue(char c)
        {
            return c switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                'A' => 14,
                _ => c - '0'
            };
        }
    }

    public class HandPart2 : HandBase
    {
        public HandPart2(string line) : base(line)
        {
        }

        protected override int GetValue(char c)
        {
            return c switch
            {
                'T' => 10,
                'J' => 0,
                'Q' => 11,
                'K' => 12,
                'A' => 13,
                _ => c - '0'
            };
        }

        protected override List<int> GetGroupCount()
        {
            var group = Cards.GroupBy(c => c).Select(g => g).ToList();
            var countJ = 0;
            var groupJ = group.FirstOrDefault(g => g.Key == 0);
            if (groupJ != null)
            {
                countJ = groupJ.Count();
            }

            var result = group.Where(g => g.Key != 0)
                .OrderByDescending(g => g.Count())
                .Select(g =>
                {
                    var cnt = g.Count() + countJ;
                    countJ = 0;
                    return cnt;
                })
                .ToList();

            if (result.Count == 0)
            {
                result.Add(5);
            }

            if (result.Sum() != 5)
            {
                throw new InvalidDataException("group");
            }
            return result;
        }
    }

    public abstract class HandBase
    {
        public List<int> Cards { get; }
        public int Bid { get; }
        public string HandValues { get; }
        public HandValue Weight { get; }
        public double Value { get; private set; }

        protected HandBase(string line)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            HandValues = parts[0];
            Cards = parts[0].Select(GetValue).ToList();
            Bid = parts[1].ToInt();
            Weight = GetWeight();
        }

        protected abstract int GetValue(char c);

        private HandValue GetWeight()
        {
            var group = GetGroupCount();
            Value = Cards.Select((c, index) => Math.Pow(14, 5 - index) * c).Sum();
            if (IsFiveOfAKind(group))
            {
                return HandValue.FiveOfAKind;
            }
            if (IsFourOfAKind(group))
            {
                return HandValue.FourOfAKind;
            }

            if (IsFullHouse(group))
            {
                return HandValue.FullHouse;
            }

            if (IsThreeOfAKind(group))
            {
                return HandValue.ThreeOfAKind;
            }

            if (IsTwoPair(group))
            {
                return HandValue.TwoPair;
            }

            if (IsPair(group))
            {
                return HandValue.Pair;
            }
            return HandValue.HighCard;
        }

        protected virtual List<int> GetGroupCount()
        {
            return Cards.GroupBy(c => c).Select(g => g.Count()).ToList();
        }

        public bool IsFiveOfAKind(List<int> groupCount)
        {
            return groupCount.Any(g => g == 5);
        }

        public bool IsFourOfAKind(List<int> groupCount)
        {
            return groupCount.Any(g => g == 4);
        }

        public bool IsThreeOfAKind(List<int> groupCount)
        {
            return groupCount.Any(g => g == 3);
        }

        public bool IsFullHouse(List<int> groupCount)
        {
            return groupCount.Any(g => g == 3) && groupCount.Any(g => g == 2);
        }

        public bool IsTwoPair(List<int> groupCount)
        {
            return groupCount.Count(g => g == 2) == 2;
        }

        public bool IsPair(List<int> groupCount)
        {
            return groupCount.Any(g => g == 2);
        }

        public override string ToString()
        {
            return $"{HandValues} {Bid} ({Weight} {Value})";
        }
    }



    [Day(ExpectedValue = "5905", ShouldLog = true)]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            return input.GetLines()
                .Select(c => new HandPart2(c))
                .OrderBy(h => h.Weight).ThenBy(h => h.Value)
                .DumpLine("hands")
                .Select((h, index) => h.Bid * (index + 1))
                .Sum().ToString();
        }
        
    }
}
    