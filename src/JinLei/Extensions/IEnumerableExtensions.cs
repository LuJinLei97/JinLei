using System.Collections;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault([]);

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Any() == false;

    public static IEnumerable<KeyValuePair<int, TSource>> SelectIndexValue<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Select((t, i) => new KeyValuePair<int, TSource>(i, t));

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange<TSource>(this IEnumerable<TSource> items, int index) => 0 <= index && index <= items.CountOrZero() - 1;

    public static TCollection ToTCollectionOrEmpty<TCollection, TSource>(this IEnumerable<TSource> items) where TCollection : ICollection<TSource>, new() => items is TCollection collection ? collection.GetSelfOrEmpty() : [.. items.GetSelfOrEmpty()];

    public static LinkedList<TSource> ToLinkedListOrEmpty<TSource>(this IEnumerable<TSource> items) => items.ToTCollectionOrEmpty<LinkedList<TSource>, TSource>();
}