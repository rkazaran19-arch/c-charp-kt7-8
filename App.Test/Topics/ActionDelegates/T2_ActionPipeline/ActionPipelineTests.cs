using NUnit.Framework.Legacy;

// Ожидается реализация в App.Topics.ActionDelegates.T2_ActionPipeline
namespace App.Test_.Topics.ActionDelegates.T2_ActionPipeline;

[TestFixture]
public class ActionPipelineTests
{
    [Test]
    public void InvokeAll_InvokesInOrder_ReturnsCount()
    {
        var called = new List<string>();
        Action<string> a = s => called.Add("A:" + s);
        Action<string> b = s => called.Add("B:" + s);
        Action<string> c = s => called.Add("C:" + s);

        var count = App.Topics.ActionDelegates.T2_ActionPipeline.ActionPipeline.InvokeAll(
            "x",
            a, b, c
        );

        CollectionAssert.AreEqual(new[] {"A:x", "B:x", "C:x"}, called);
        Assert.That(count, Is.EqualTo(3));
    }

    [Test]
    public void InvokeAll_NullArray_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            App.Topics.ActionDelegates.T2_ActionPipeline.ActionPipeline.InvokeAll("x", null!));
    }

    [Test]
    public void InvokeAll_SkipNullHandlers()
    {
        var called = new List<string>();
        Action<string> a = s => called.Add("A:" + s);
        Action<string>? b = null;

        var count = App.Topics.ActionDelegates.T2_ActionPipeline.ActionPipeline.InvokeAll("z", a, b!, a);

        CollectionAssert.AreEqual(new[] {"A:z", "A:z"}, called);
        Assert.That(count, Is.EqualTo(2));
    }

    [Test]
    public void InvokeAll_StopOnException_ReThrow()
    {
        var called = new List<string>();
        Action<string> a = s => called.Add("A:" + s);
        Action<string> bad = _ => throw new InvalidOperationException("bad");
        Action<string> c = s => called.Add("C:" + s);

        var ex = Assert.Throws<InvalidOperationException>(() =>
            App.Topics.ActionDelegates.T2_ActionPipeline.ActionPipeline.InvokeAll("x", a, bad, c));
        Assert.That(ex!.Message, Is.EqualTo("bad"));
        CollectionAssert.AreEqual(new[] {"A:x"}, called); // C не должен быть вызван
    }

    [Test]
    public void InvokeAll_WithMulticastDelegate_Works()
    {
        var called = new List<int>();
        Action<string> a = _ => called.Add(1);
        Action<string> b = _ => called.Add(2);
        var multi = (Action<string>)Delegate.Combine(a, b);

        var count = App.Topics.ActionDelegates.T2_ActionPipeline.ActionPipeline.InvokeAll("q", multi);
        CollectionAssert.AreEqual(new[] {1, 2}, called);
        Assert.That(count, Is.EqualTo(1)); // вызван один делегат (но внутри — 2 цели)
    }
}
