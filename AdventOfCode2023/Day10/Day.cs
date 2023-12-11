using AdventOfCode.Lib;

namespace AdventOfCode2023.Day10
{

    public class PuzzleBase
    {
        protected static void FillValues(string[] table, (int x, int y) position, List<(int x, int y)> values)
        {
            var c = table[position.y][position.x];

            switch (c)
            {
                case '|':
                    values.Add((position.x, position.y + 1));
                    values.Add((position.x, position.y - 1));
                    break;
                case '-':
                    values.Add((position.x + 1, position.y));
                    values.Add((position.x - 1, position.y));

                    break;
                case 'L':
                    values.Add((position.x, position.y - 1));
                    values.Add((position.x + 1, position.y));

                    break;
                case 'J':
                    values.Add((position.x, position.y - 1));
                    values.Add((position.x - 1, position.y));
                    break;
                case '7':
                    values.Add((position.x, position.y + 1));
                    values.Add((position.x - 1, position.y));
                    break;
                case 'F':
                    values.Add((position.x, position.y + 1));
                    values.Add((position.x + 1, position.y));
                    break;
                case 'S':

                    foreach (var point in table.GetAllSurroundingWithCoordinates(position.x, position.y))
                    {
                        switch (point.c)
                        {
                            case '|':
                                if (point.x == position.x)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                            case '-':
                                if (point.y == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                            case 'L':
                                if (point.x == position.x && point.y - 1 == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                if (point.x - 1 == position.x && point.y == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                            case 'J':
                                if (point.x == position.x && point.y + 1 == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                if (point.x - 1 == position.x && point.y == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                            case '7':
                                if (point.x == position.x && point.y + 1 == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                if (point.x - 1 == position.x && point.y == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                            case 'F':
                                if (point.x == position.x && point.y + 1 == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                if (point.x + 1 == position.x && point.y == position.y)
                                {
                                    values.Add((point.x, point.y));
                                }

                                break;
                        }
                    }

                    break;
            }
        }

        protected static void CompletePosition(string[] table, (int x, int y) position, Dictionary<(int x, int y), List<(int x, int y)>> visited)
        {
            Stack<(int x, int y)> toProcess = new Stack<(int x, int y)>();
            toProcess.Push(position);
            while (toProcess.Any())
            {
                var currentPosition = toProcess.Pop();

                if (visited.ContainsKey(currentPosition))
                {
                    continue;
                }

                var values = new List<(int x, int y)>();
                visited.Add(currentPosition, values);
                FillValues(table, currentPosition, values);
                foreach (var valueTuple in values)
                {
                    toProcess.Push(valueTuple);
                }
            }
        }
    }

    [Day(ExpectedValue = "8")]
    public class Puzzle1 : PuzzleBase,  IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            Dictionary<(int x, int y), List< (int x, int y) >> visited = new Dictionary<(int x, int y), List<(int x, int y)>>();
            var table = input.GetLines().ToArray();

            var s = table.SelectMany((s, y) => s.Select((c, x) => (x, y, c))).First(c => c.c == 'S');
            var position = (s.x, s.y);
            
            CompletePosition(table, position, visited);

            return (visited.Count / 2).ToString();
        }

    }

    [Day(ExpectedValue = "8", SampleInput = "testInput2.txt")]
    public class Puzzle2 : PuzzleBase, IDay
    {
        public string GetPuzzle(string input, bool isRealCase)
        {
            Dictionary<(int x, int y), List<(int x, int y)>> visited = new Dictionary<(int x, int y), List<(int x, int y)>>();
            var table = input.GetLines().ToArray();

            var s = table.SelectMany((s, y) => s.Select((c, x) => ((x, y, c)))).First(c => c.c == 'S');
            var position = (s.x, s.y);

            CompletePosition(table, position, visited);

            return visited.Keys.ExportImageByPath(table[0].Length, table.Length, isRealCase ? @"C:\Temp\real.bmp" : "C:\\Temp\\test.bmp").ToString();
        }
    }
}
