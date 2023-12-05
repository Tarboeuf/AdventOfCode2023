using AdventOfCode.Lib;
using AdventOfCode2023.Day5;

namespace AOC.Tests;

public class MappingProfileTests
{
    [Test]
    public void CombineCombination()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 20, Range = 10 },
            new Map { Source = 20, Destination = 30, Range = 10 },
            result
            );

        Assert.That(result.Maps, Has.Count.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(30));
        Assert.That(result.Maps[0].Range, Is.EqualTo(10));
    }

    [Test]
    public void CombineCombinationLargerFirstProfile()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 20, Range = 12 },
            new Map { Source = 20, Destination = 30, Range = 10 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(30));
        Assert.That(result.Maps[0].Range, Is.EqualTo(10));
    }

    [Test]
    public void CombineCombinationLargerSecondProfile()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 20, Range = 10 },
            new Map { Source = 20, Destination = 30, Range = 12 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(30));
        Assert.That(result.Maps[0].Range, Is.EqualTo(10));
    }

    [Test]
    public void CombineCombinationNoOnLeft()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 20, Range = 10 },
            new Map { Source = 31, Destination = 30, Range = 10 },
            result
        );
        Assert.That(result.Maps.Count, Is.EqualTo(0));
    }

    [Test]
    public void CombineCombinationNoOnRight()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 50, Range = 10 },
            new Map { Source = 31, Destination = 30, Range = 10 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(0));
    }

    [Test]
    public void CombineCombinationCrossOnLeft()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 10, Range = 12 },
            new Map { Source = 20, Destination = 21, Range = 4 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(20));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(21));
        Assert.That(result.Maps[0].Range, Is.EqualTo(2));
    }
    [Test]
    public void CombineCombinationCrossOnRight()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 20, Range = 4 },
            new Map { Source = 10, Destination = 21, Range = 12 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(31));
        Assert.That(result.Maps[0].Range, Is.EqualTo(2));
    }

    [Test]
    public void CombineCombinationContained()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 10, Destination = 110, Range = 50 },
            new Map { Source = 120, Destination = 20, Range = 20 },
            result
        );
        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(20));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(20));
        Assert.That(result.Maps[0].Range, Is.EqualTo(20));
    }
    [Test]
    public void CombineCombinationOutside()
    {
        MappingProfile result = new MappingProfile();
        Extensions.Combine(
            new Map { Source = 120, Destination = 20, Range = 20 },
            new Map { Source = 10, Destination = 110, Range = 50 },
            result
        );

        Assert.That(result.Maps.Count, Is.EqualTo(1));
        Assert.That(result.Maps[0].Source, Is.EqualTo(120));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(120));
        Assert.That(result.Maps[0].Range, Is.EqualTo(20));
    }
    [Test]
    public void TestMappingProfileCombinationOutsideMultipleRange()
    {
        MappingProfile profile1 = new MappingProfile
        {
            Maps =
            {
                new Map { Source = 120, Destination = 20, Range = 20 },
                new Map { Source = 0, Destination = 0, Range = 120 },
            }
        };

        MappingProfile profile2 = new MappingProfile
        {
            Maps = { new Map { Source = 10, Destination = 110, Range = 50 } }
        };

        var result = profile1.CombineMap(profile2);

        Assert.That(result.Maps.DumpLine().Count, Is.EqualTo(2));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(110));
        Assert.That(result.Maps[0].Range, Is.EqualTo(50));

        Assert.That(result.Maps[1].Source, Is.EqualTo(120));
        Assert.That(result.Maps[1].Destination, Is.EqualTo(120));
        Assert.That(result.Maps[1].Range, Is.EqualTo(20));

    }

    [Test]
    public void TestMappingProfileCombinationOutsideOneRange()
    {
        MappingProfile profile1 = new MappingProfile
        {
            Maps =
            {
                new Map { Source = 120, Destination = 20, Range = 20 },
            }
        };

        MappingProfile profile2 = new MappingProfile
        {
            Maps = { new Map { Source = 10, Destination = 110, Range = 50 } }
        };

        var result = profile1.CombineMap(profile2);

        Assert.That(result.Maps.DumpLine().Count, Is.EqualTo(2));
        Assert.That(result.Maps[0].Source, Is.EqualTo(10));
        Assert.That(result.Maps[0].Destination, Is.EqualTo(110));
        Assert.That(result.Maps[0].Range, Is.EqualTo(50));

        Assert.That(result.Maps[1].Source, Is.EqualTo(120));
        Assert.That(result.Maps[1].Destination, Is.EqualTo(120));
        Assert.That(result.Maps[1].Range, Is.EqualTo(20));

    }
}