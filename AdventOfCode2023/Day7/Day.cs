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

    public class Hand
    {
        public List<int> Cards { get; }
        public int Bid { get; }
        public string HandValues { get; }
        public HandValue Weight { get; }
        public double Value { get; private set; }

        public Hand(string line)
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            HandValues = parts[0];
            Cards = parts[0].Select(c => char.IsDigit(c) && c > '1' ? c - '0' : c switch
            {
                'T' => 10,
                'J' => 11,
                'Q' => 12,
                'K' => 13,
                '1' => 14,
                'A' => 14,
                _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
            } )
                .ToList();
            Bid = parts[1].ToInt();
            Weight = GetWeight();
        }

        private HandValue GetWeight()
        {
            var group = Cards.GroupBy(c => c).ToList();
            //Value = group.OrderBy(g => g.Count())
            //    .ThenBy(g => g.Key)
            //    .Select((c, index) => Math.Pow(14, index) * c.Key).Sum();
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

        public bool IsFiveOfAKind(List<IGrouping<int, int>> group)
        {
            return group.Any(g => g.Count() == 5);
        }

        public bool IsFourOfAKind(List<IGrouping<int, int>> group)
        {
            return group.Any(g => g.Count() == 4);
        }
        public bool IsThreeOfAKind(List<IGrouping<int, int>> group)
        {
            return group.Any(g => g.Count() == 3);
        }

        public bool IsFullHouse(List<IGrouping<int, int>> group)
        {
            return group.Any(g => g.Count() == 3) && group.Any(g => g.Count() == 2);
        }

        public bool IsTwoPair(List<IGrouping<int, int>> group)
        {
            return group.Count(g => g.Count() == 2) == 2;
        }

        public bool IsPair(List<IGrouping<int, int>> group)
        {
            return group.Any(g => g.Count() == 2);
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
                .Select(c => new Hand(c))
                .OrderBy(h => h.Weight).ThenBy(h => h.Value)
                .Select((h, index) => h.Bid * (index + 1))
                .Sum().ToString();
        }
        
    }
}
    