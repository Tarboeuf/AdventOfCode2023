using AdventOfCode.Lib;

namespace AdventOfCode2023.Day8
{

    [Day(ExpectedValue = "6")]
    public class Puzzle1 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var nodes = GetNodes(input, out var path);

            int inc = 0;
            Node current = nodes["AAA"];
            while (true)
            {
                switch (path[inc++%path.Length])
                {
                    case 'R':
                        current = current.Right.Value;
                        break;
                    case 'L':
                        current = current.Left.Value;
                        break;
                }

                if (current.Current == "ZZZ")
                {
                    return (inc).ToString();
                }
            }
        }

        public static Dictionary<string, Node> GetNodes(string input, out string path)
        {
            var lines = input.GetLines();
            using var enumerator = lines.GetEnumerator();
            enumerator.MoveNext();
            path = enumerator.Current;
            var nodes = new Dictionary<string, Node>();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current == "")
                {
                    continue;
                }

                string[] parts = enumerator.Current.Split(" = ");
                var nodesName = parts[1].Replace("(", "").Replace(")", "").Split(",").Select(n => n.Trim()).ToArray();
                nodes.Add(parts[0], new Node(parts[0], () => nodes[nodesName[0]], () => nodes[nodesName[1]]));
            }

            return nodes;
        }
    }

    public class Node
    {
        public Node(string current, Func<Node> left, Func<Node> right)
        {
            Current = current;
            Left = new Lazy<Node>(left);
            Right = new Lazy<Node>(right);
        }

        public string Current { get; set; }
        public Lazy<Node> Left { get; set; }
        public Lazy<Node> Right { get; set; }

    }

    [Day(ExpectedValue = "6", SampleInput = "testInput2.txt")]
    public class Puzzle2 : IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            var nodes = Puzzle1.GetNodes(input, out var path);

            int inc = 0;
            var currents = nodes.Values.Where(n => n.Current.EndsWith("A")).ToArray();
            var recurrences = new ulong?[currents.Length];
            while (true)
            {
                switch (path[inc++ % path.Length])
                {
                    case 'R':
                        for (int i = 0; i < currents.Length; i++)
                        {
                            if (recurrences[i] != null)
                            {
                                continue;
                            }

                            currents[i] = currents[i].Right.Value;
                            recurrences[i] = CheckZReached(currents[i], inc);
                        }
                        break;
                    case 'L':
                        for (int i = 0; i < currents.Length; i++)
                        {
                            if (recurrences[i] != null)
                            {
                                continue;
                            }

                            currents[i] = currents[i].Left.Value;
                            CheckZReached(currents[i], inc);
                        }
                        break;
                }

                if (recurrences.All(n => n != null))
                {
                    return recurrences.Select(r => r!.Value).Aggregate(MyEnumerable.LCM).ToString();
                }
            }
        }

        private ulong? CheckZReached(Node current, int inc)
        {
            if (current.Current.EndsWith("Z"))
            {
                return (ulong)inc;
            }

            return null;
        }
    }
}
    