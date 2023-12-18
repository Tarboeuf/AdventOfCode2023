using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;

namespace AdventOfCode.Lib;

public static class Conf
{
    public static bool IsDump { get; set; } = true;
}

public static class MyEnumerable
{
    public static IEnumerable<(int x, int y)> GetTableValues(int maxX, int maxY, int startX = 0, int startY = 0)
    {
        for (var y = startX; y < maxY; y++)
        {
            for (var x = startY; x < maxX; x++)
            {
                yield return (x, y);
            }
        }
    }

    public static IEnumerable<(T, T)> GetAllPairs<T>(this IEnumerable<T> pairs)
    {
        var list = pairs.ToList();
        for (var i = 0; i < list.Count; i++)
        {
            for (var j = i + 1; j < list.Count; j++)
            {
                yield return (list[i], list[j]);
            }
        }
    }
    
    public static IEnumerable<T> Include<T>(this IEnumerable<T> values, T value)
    {
        foreach (var item in values)
        {
            yield return item;
        }
        yield return value;
    }

    public static ulong Product(this IEnumerable<int> values)
    {
        ulong result = 1;
        foreach (var value in values)
        {
            result *= (ulong)value;
        }
        return result;
    }

    public static ulong LCM(ulong a, ulong b)
    {
        return (a / GCD(a, b)) * b;
    }

    public static ulong GCD(ulong a, ulong b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
    {
        foreach (var value in values)
        {
            action(value);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> values, Action<(T item, T? previous)> action)
    {
        T? previous = default;
        foreach (var value in values)
        {
            action((value, previous));
            previous = value;
        }
    }

    public static bool IsIn<T>(this T value, params T[] values)
    {
        return values.Contains(value);
    }

    public static IEnumerable<string> GetLines(this string input, string extraSplit = "")
    {
        foreach (var item in input.Split(Environment.NewLine + extraSplit, StringSplitOptions.RemoveEmptyEntries))
        {
            yield return item;
        }
    }
    
    public static List<List<char>> GetColumns(this string input, string extraSplit = "")
    {
        List<List<char>> result = new List<List<char>>();
        foreach (var item in input.Split(Environment.NewLine + extraSplit, StringSplitOptions.RemoveEmptyEntries))
        {
            if (result.Count == 0)
            {
                foreach (var t in item)
                {
                    result.Add(new List<char> {t});
                }
            }
            else
            {
                for (var i = 0; i < item.Length; i++)
                {
                    result[i].Add(item[i]);
                }
            }
        }

        return result;
    }

    public static IEnumerable<int> GetIndexesOf(this List<char> input, char value)
    {
        for (var i = 0; i < input.Count; i++)
        {
            if (input[i] == value)
            {
                yield return i;
            }
        }
    }

    public static IEnumerable<int> GetIndexesOf(this string input, char value)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (input[i] == value)
            {
                yield return i;
            }
        }
    }
    
    public static IEnumerable<char> GetAllSurrounding(this string[] lines, int x, int y, int length)
    {
        for (var i = y - 1; i <= y + 1; i++)
        {
            for (var j = x - 1; j <= x + length; j++)
            {
                if (i == y && j >= x && j <= (x + length - 1))
                {
                    continue;
                }

                if (i < 0 || j < 0 || i >= lines.Length || j >= lines[i].Length)
                {
                    continue;
                }
                yield return lines[i][j];
            }
        }
    }

    public static IEnumerable<(char c, int x, int y)> GetAllSurroundingWithCoordinates(this string[] lines, int x, int y)
    {
        for (var Y = y - 1; Y <= y + 1; Y++)
        {
            for (var X = x - 1; X <= x + 1; X++)
            {
                if (Y < 0 || X < 0 || Y >= lines.Length || X >= lines[Y].Length || x == X && y == Y)
                {
                    continue;
                }
                yield return (lines[Y][X], X, Y);
            }
        }
    }

    public static IEnumerable<NumberPositioned> GetAllSurroundingNumbers(this string[] lines, int x, int y)
    {
        for (var i = y - 1; i <= y + 1; i++)
        {
            for (var j = x - 1; j <= x + 1; j++)
            {
                if (i == y && j == x)
                {
                    continue;
                }

                if (i < 0 || j < 0 || i >= lines.Length || j >= lines[i].Length)
                {
                    continue;
                }

                if (char.IsDigit(lines[i][j]))
                {
                    yield return lines[i].GetNumberPositioned(j, i);
                }
            }
        }
    }

    public static NumberPositioned GetNumberPositioned(this string line, int x, int y)
    {
        int start = x, end = x;
        for (var i = x; i >= 0; i--)
        {
            if (!char.IsDigit(line[i]))
            {
                break;
            }
            start = i;
        }

        for (var i = x; i < line.Length; i++)
        {
            if (!char.IsDigit(line[i]))
            {
                break;
            }
            end = i + 1;
        }
        return new NumberPositioned { Number = int.Parse(line[start..end]), X = start, Y = y };
    }

    public static IEnumerable<long> EnumerateTo(this long start, long range)
    {
        for (long i = 0; i <= range; i++)
        {
            yield return start + i;
        }
    }

    public static T DumpLine<T>(this T value, string? entry = null)
    {
        if (!Conf.IsDump)
        {
            return value;
        }

        value.Dump(entry);
        Debug.WriteLine("");
        return value;
    }

    public static IEnumerable<(T first, T second)> GetInPairs<T>(this IEnumerable<T> values)
    {
        using var enumerator = values.GetEnumerator();
        while (enumerator.MoveNext())
        {
            var previous = enumerator.Current;
            if (!enumerator.MoveNext())
            {
                yield break;
            }
            yield return (previous, enumerator.Current);
        }
    }

    public static T Dump<T>(this T value, string? entry = null)
    {
        if (!Conf.IsDump)
        {
            return value;
        }
        var str = ToString(value);

        entry = entry == null ? null : $"{entry} : ";
        Debug.Write($"{entry}{str}");
        return value;
    }

    public static string ToString(object? value, int inc = 0)
    {
        var str = value?.ToString() ?? "null";
        if (value is IDictionary dictionary)
        {
            List<string> values = new List<string>();
            for (var i = 0; i < dictionary.Count; i++)
            {
                values.Add(
                    $"{dictionary.Keys.OfType<object>().ElementAt(i)} : {ToString(dictionary.Values.OfType<object>().ElementAt(i), inc + 1)}");
            }
            str = $"\r\n{"  ".Repeat(inc)}#{string.Join($"\r\n{"  ".Repeat(inc)}#", values)}";
        }
        else if (value is IEnumerable values && value is not string)
        {
            var e = values.OfType<object>().ToList();
            str = $"\r\n{"  ".Repeat(inc)}-{string.Join($"\r\n{"  ".Repeat(inc)}-", e.Select((v, index) => $"[{index}]{ToString(v, inc + 1)}"))} ({e.Count})";
        }

        return str;
    }

    public static List<T> Replace<T>(this IEnumerable<T> values, T oldValue, T newValue)
    {
        var result = new List<T>();
        foreach (var item in values)
        {
            result.Add(item != null && item.Equals(oldValue) ? newValue : item);
        }
        return result;
    }

    public static string Repeat(this string value, int count, string inBetween = null)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < count; i++)
        {
            sb.Append(value);
            if (inBetween != null)
            {
                sb.Append(inBetween);
            }
        }

        if (inBetween != null && sb.Length > inBetween.Length)
        {
            sb.Remove(sb.Length - inBetween.Length, inBetween.Length);
        }
        return sb.ToString();
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> values, int count)
    {
        var enumerable = values.ToList();
        for (var i = 0; i < count; i++)
        {
            foreach (var item in enumerable)
            {
                yield return item;
            }
        }
    }

    public static int ToInt(this string value)
    {
        return int.Parse(value);
    }

    public static long ToLong(this string value)
    {
        return long.Parse(value);
    }

    public static long Multiply(this IEnumerable<int> values)
    {
        long result = 1;
        foreach (long value in values)
        {
            result *= value;
        }
        return result;
    }

    public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this List<T> list, int count)
    {
        var i = 0;
        if (count == 0)
        {
            yield return Enumerable.Empty<T>();
            yield break;
        }
        foreach (var item in list)
        {
            if (count == 1)
                yield return new[] { item };
            else
            {
                foreach (var result in GetPermutations(list.Skip(i + 1).ToList(), count - 1))
                    yield return result.Prepend(item);
            }

            ++i;
        }
    }

    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrEmpty(value);
    }

    public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> values, Func<T, bool> predicate)
    {
        var list = new List<T>();
        foreach (var item in values)
        {
            if (predicate(item))
            {
                yield return list;
                list = new List<T>();
            }
            else
            {
                list.Add(item);
            }
        }
        yield return list;
    }

    public record NumberPositioned
    {
        public int Number { get; init; }
        public int X { get; init; }
        public int Y { get; init; }
    }
}

public class ListComparer<T> : IEqualityComparer<List<T>>
{
    private readonly Func<T, long> _unique;

    public ListComparer(Func<T, long> unique)
    {
        _unique = unique;
    }

    public Dictionary<List<T>, long> Cache { get; } = new();

    public bool Equals(List<T>? x, List<T>? y)
    {
        if (x == null || y == null)
        {
            return false;
        }
        return x.SequenceEqual(y);
    }

    public int GetHashCode([DisallowNull] List<T> obj)
    {
        unchecked
        {
            int hash = 19;
            foreach (var foo in obj)
            {
                hash = hash * 31 + _unique(foo).GetHashCode();
            }
            return hash;
        }
    }
}