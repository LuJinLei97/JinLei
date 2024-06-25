using System.Collections;

namespace JinLei.Extensions;
public static partial class IEnumerableExtensions
{
    public static IEnumerable<TSource> GetSelfOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetValueOrDefault([]);

    public static TEnumerable GetSelfOrEmpty<TEnumerable>(this TEnumerable items) where TEnumerable : IEnumerable, new() => items.GetValueOrDefault([]);

    public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Any() == false;

    public static IEnumerable<KeyValuePair<int, TSource>> SelectIndexValue<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Select((t, i) => KeyValuePair.Create(i, t));

    public static int CountOrZero<TSource>(this IEnumerable<TSource> items) => items.GetSelfOrEmpty().Count();

    public static bool CheckRange<TSource>(this IEnumerable<TSource> items, int index) => 0 <= index && items is ICollection<TSource> collection ? index <= collection.CountOrZero() - 1 : items.SelectIndexValue().Any(t => t.Key == index);
}