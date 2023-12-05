using AdventOfCode.Lib;
using AoC.Day1;
using System.Diagnostics;
using System.Reflection;

namespace AOC.Tests
{
    [TestFixture]
    public class UnitTest
    {
        [TestCaseSource(nameof(Scenarios))]
        public void TestPuzzle(TestScenario scenario)
        {
            string input = File.OpenText(scenario.SamplePath).ReadToEnd();
            Conf.IsDump = true;
            var result = scenario.Day.GetPuzzle(input, false);
            Assert.That(result, Is.EqualTo(scenario.ExpectedResult));


            Conf.IsDump = scenario.AttributeShouldLog;
            input = File.OpenText(scenario.RealPath).ReadToEnd();
            result = scenario.Day.GetPuzzle(input, true);
            Console.WriteLine(scenario.Day.GetType().Name);
            Console.WriteLine(result);
        }

        [TestCase(@"aze
qsd
wxc", 1, 1, 1, ExpectedResult = "azeqdwxc")]
        [TestCase(@"aze
qsd
wxc", 1, 1, 2, ExpectedResult = "azeqwxc")]
        [TestCase(@"azer
qsdf
wxcv", 0, 1, 2, ExpectedResult = "azedwxc")]
        [TestCase(
            @"4671.
23456
7.35.", 0, 0, 3, ExpectedResult = "12345")]
        public string GetAllSurroundingTest(string input, int x, int y, int length)
        {
            return string.Concat(input.GetLines().ToArray().GetAllSurrounding(x, y, length));
        }

        [TestCase("....234..", 4, 1, ExpectedResult = 234)]
        [TestCase("....234..", 5, 1, ExpectedResult = 234)]
        [TestCase("....234", 5, 1, ExpectedResult = 234)]
        [TestCase("....234", 6, 1, ExpectedResult = 234)]
        [TestCase("234...", 0, 1, ExpectedResult = 234)]
        [TestCase("234...", 1, 1, ExpectedResult = 234)]
        [TestCase("234...", 2, 1, ExpectedResult = 234)]
        public int GetNumberPositionedTest(string line, int x, int y)
        {
            return line.GetNumberPositioned(x, y).Number;
        }

        public static TestScenario[] Scenarios = GetScenarios().ToArray();


        public static IEnumerable<TestScenario> GetScenarios()
        {
            var day = typeof(IDay);
            string basePath = "";
            var days = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => day.IsAssignableFrom(t));
            yield return new TestScenario(new Day01Puzzle2(), "", "", "", "", false);
            foreach (var item in days.OrderBy(d => d.Name))
            {
                if (item == day)
                {
                    continue;
                }

                var attribute = item.GetCustomAttribute<DayAttribute>();
                if (attribute == null)
                {
                    continue;
                }
                if (item.Namespace == null)
                {
                    throw new InvalidDataException("namespace");
                }

                yield return
                    new TestScenario(
                        GetDay(item),
                        attribute.ExpectedValue!,
                        $"{item.Namespace?.Split(".", StringSplitOptions.RemoveEmptyEntries).Last()}.{item.Name}",
                        Path.Combine(basePath, item.Namespace.Split('.').Last(), attribute.SampleInput),
                        Path.Combine(basePath, item.Namespace.Split('.').Last(), "input.txt"),
                        attribute.ShouldLog);
            }
        }

        private static IDay GetDay(Type iDay)
        {
            try
            {
                return Activator.CreateInstance(iDay) is not IDay day ? throw new InvalidDataException("day") : day;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ex : {ex}");
            }
        }

        [SetUp]
        public void StartTest()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
        }

        [TearDown]
        public void EndTest()
        {
            Trace.Flush();
        }
    }
}