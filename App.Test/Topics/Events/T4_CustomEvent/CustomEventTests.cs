using NUnit.Framework.Legacy;

// Ожидается реализация в App.Topics.Events.T4_CustomEvent
namespace App.Test_.Topics.Events.T4_CustomEvent;

[TestFixture]
public class CustomEventTests
{
    [Test]
    public void Counter_BasicThreshold_FiresOnMultiples()
    {
        var fired = new List<int>();
        var c = new App.Topics.Events.T4_CustomEvent.Counter(threshold: 5);
        c.ThresholdReached += (_, e) => fired.Add(e.Value);

        c.Increment(3); // 3
        c.Increment(2); // 5 → fire
        c.Increment(5); // 10 → fire

        CollectionAssert.AreEqual(new[] {5, 10}, fired);
    }

    [Test]
    public void Counter_AddRemoveHandlers_RespectLimitsAndNoDuplicates()
    {
        var c = new App.Topics.Events.T4_CustomEvent.Counter(5, maxSubscribers: 2);
        EventHandler<App.Topics.Events.T4_CustomEvent.ThresholdReachedEventArgs> h1 = (_, _) => { };
        EventHandler<App.Topics.Events.T4_CustomEvent.ThresholdReachedEventArgs> h2 = (_, _) => { };
        EventHandler<App.Topics.Events.T4_CustomEvent.ThresholdReachedEventArgs> h3 = (_, _) => { };

        // add first time ok
        c.ThresholdReached += h1;
        // duplicate ignored
        c.ThresholdReached += h1;
        // second distinct ok
        c.ThresholdReached += h2;
        // third should fail due to limit
        Assert.Throws<InvalidOperationException>(() => c.ThresholdReached += h3);

        // remove and add again
        c.ThresholdReached -= h1;
        Assert.DoesNotThrow(() => c.ThresholdReached += h3);
    }

    [Test]
    public void Counter_AddNull_Throws_RemoveNull_Ignored()
    {
        var c = new App.Topics.Events.T4_CustomEvent.Counter(5);
        Assert.Throws<ArgumentNullException>(() => c.ThresholdReached += null!);
        Assert.DoesNotThrow(() => c.ThresholdReached -= null!);
    }

    [Test]
    public void Counter_InvalidArgs_Throw()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new App.Topics.Events.T4_CustomEvent.Counter(0));
        var c = new App.Topics.Events.T4_CustomEvent.Counter(5);
        Assert.Throws<ArgumentOutOfRangeException>(() => c.Increment(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => c.Increment(-1));
    }
}
