

// Ожидается реализация в App.Topics.FuncDelegates.T3_FuncFilterMap
namespace App.Test_.Topics.FuncDelegates.T3_FuncFilterMap;

[TestFixture]
public class FuncFilterMapTests
{
    [Test]
    public void FilterMap_BasicScenario()
    {
        var data = new[] {1, 2, 3, 4, 5};
        var res = App.Topics.FuncDelegates.T3_FuncFilterMap.LinqLite
            .FilterMap(data, x => x % 2 == 1, x => x * x)
            .ToArray();
        Assert.That(res, Is.EqualTo(new[] {1, 9, 25}));
    }

    [Test]
    public void FilterMap_Empty_ReturnsEmpty()
    {
        var res = App.Topics.FuncDelegates.T3_FuncFilterMap.LinqLite
            .FilterMap(Array.Empty<int>(), x => true, x => x)
            .ToArray();
        Assert.That(res, Is.Empty);
    }

    [Test]
    public void FilterMap_Nulls_Throw()
    {
        Assert.Throws<ArgumentNullException>(() => App.Topics.FuncDelegates.T3_FuncFilterMap.LinqLite.FilterMap<int, int>(null!, _ => true, x => x));
        Assert.Throws<ArgumentNullException>(() => App.Topics.FuncDelegates.T3_FuncFilterMap.LinqLite.FilterMap([1], null!, x => x));
    }

    [Test]
    public void FilterMap_ReferenceTypes_SelectProperties()
    {
        var people = new[]
        {
            new Person("Ann", 22),
            new Person("Bob", 17),
            new Person("Cat", 30)
        };
        var names = App.Topics.FuncDelegates.T3_FuncFilterMap.LinqLite
            .FilterMap(people, p => p.Age >= 18, p => p.Name)
            .ToArray();
        Assert.That(names, Is.EqualTo(new[] {"Ann", "Cat"}));
    }

    private sealed record Person(string Name, int Age);
}
