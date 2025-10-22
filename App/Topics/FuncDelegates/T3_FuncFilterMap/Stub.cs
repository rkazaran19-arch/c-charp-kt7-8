namespace App.Topics.FuncDelegates.T3_FuncFilterMap;

public class LinqLite
{
    public static IEnumerable<TResult> FilterMap<T, TResult>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, TResult> selector)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));
        if (selector == null)
            throw new ArgumentNullException(nameof(selector));

        return FilterMapIterator(source, predicate, selector);
    }

    private static IEnumerable<TResult> FilterMapIterator<T, TResult>(IEnumerable<T> source, Func<T, bool> predicate, Func<T, TResult> selector)
    { 
        foreach (T item in source)
        {
            if (predicate(item))
            {
                yield return selector(item);
            }
        }
    }
}

