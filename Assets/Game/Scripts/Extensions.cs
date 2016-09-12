using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extensions
{
    public static IEnumerable<KeyValuePair<T, U>> Zip<T, U>(this IEnumerable<T> first, IEnumerable<U> second)
    {
        IEnumerator<T> firstEnumerator = first.GetEnumerator();
        IEnumerator<U> secondEnumerator = second.GetEnumerator();

        while (firstEnumerator.MoveNext())
        {
            if (secondEnumerator.MoveNext())
            {
                yield return new KeyValuePair<T, U>(firstEnumerator.Current, secondEnumerator.Current);
            }
            else
            {
                yield return new KeyValuePair<T, U>(firstEnumerator.Current, default(U));
            }
        }
        while (secondEnumerator.MoveNext())
        {
            yield return new KeyValuePair<T, U>(default(T), secondEnumerator.Current);
        }
    }

    public static IEnumerable<T> Concat<T>(this List<T> first, List<T> second)
    {
        IEnumerator<T> firstEnumerator = first.GetEnumerator();
        IEnumerator<T> secondEnumerator = second.GetEnumerator();

        while (firstEnumerator.MoveNext())
        {
            yield return firstEnumerator.Current;
        }

        while (secondEnumerator.MoveNext())
        {
            yield return secondEnumerator.Current;
        }
    }

}
