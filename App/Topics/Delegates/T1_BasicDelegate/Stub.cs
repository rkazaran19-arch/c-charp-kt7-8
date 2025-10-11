using System.Security.Cryptography;

namespace App.Topics.Delegates.T1_BasicDelegate;

public delegate int IntUnary(int x);
public delegate bool IntPredicate(int x);

public class IntAlgorithms
{
    public static int[] Map(int[] source, IntUnary f)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (f == null)
        {
            throw new ArgumentNullException();
        }

        if (source.Length == 0)
        {
            return new int[0];
        }

        int[] result = new int[source.Length];
        for (int index = 0; index < source.Length; index++)
        {
            result[index] = f(source[index]);
        }

        return result;
    }

    public static int[] Filter(int[] source, IntPredicate predicate)
    {
        if (source == null)
        {
            throw new ArgumentNullException("source");
        }

        if (predicate == null)
        {
            throw new ArgumentNullException();
        }

        if(source.Length == 0)
        {
            return new int[0];
        }

        int[] result2 = new int[source.Length];
        int num = 0;

        for (int index = 0; index < source.Length; index++)
        {
            if (predicate(source[index]))
            {
                result2[num++] = index;
            }
        }

        int[] result = new int[num];
        Array.Copy(result2, result, num);

        return result;
    }
}