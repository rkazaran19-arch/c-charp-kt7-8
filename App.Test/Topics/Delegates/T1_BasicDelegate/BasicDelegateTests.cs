

// Ожидается реализация в App.Topics.Delegates.T1_BasicDelegate
namespace App.Test_.Topics.Delegates.T1_BasicDelegate;

[TestFixture]
public class BasicDelegateTests
{
    // Предполагаемые сигнатуры, которые студент должен реализовать в проекте App:
    // namespace App.Topics.Delegates.T1_BasicDelegate;
    // public delegate int IntUnary(int x);
    // public delegate bool IntPredicate(int x);
    // public static class IntAlgorithms
    // {
    //     public static int[] Map(int[] source, IntUnary f);
    //     public static int[] Filter(int[] source, IntPredicate predicate);
    // }

    [Test]
    public void Map_DoublesValues_ReturnsNewArray()
    {
        var data = new[] {1, 2, 3};
        var result = App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Map(
            data, x => x * 2);

        Assert.That(result, Is.EqualTo(new[] {2, 4, 6}));
        Assert.That(ReferenceEquals(result, data), Is.False, "Should return new array, not modify source");
    }

    [Test]
    public void Filter_KeepEven_ReturnsOnlyEven()
    {
        var data = new[] {-3, -2, -1, 0, 1, 2, 3};
        var result = App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Filter(
            data, x => x % 2 == 0);

        Assert.That(result, Is.EqualTo(new[] {-2, 0, 2}));
    }

    [Test]
    public void Map_SourceNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Map(null!, x => x));
    }

    [Test]
    public void Map_DelegateNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Map(new[] {1}, null!));
    }

    [Test]
    public void Filter_PredicateNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Filter(new[] {1}, null!));
    }

    [Test]
    public void EmptyInput_ReturnsEmpty()
    {
        var mapped = App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Map(Array.Empty<int>(), x => x + 1);
        var filtered = App.Topics.Delegates.T1_BasicDelegate.IntAlgorithms.Filter(Array.Empty<int>(), x => x > 0);
        Assert.That(mapped, Is.Empty);
        Assert.That(filtered, Is.Empty);
    }
}
